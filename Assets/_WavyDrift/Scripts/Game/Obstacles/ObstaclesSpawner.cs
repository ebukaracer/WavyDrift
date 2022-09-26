using Racer.Utilities;
using System.Collections;
using Racer.ObjectPooler;
using UnityEngine;

internal class ObstaclesSpawner : MultipleSpawner
{
    private PoolObject _spawnedPoolObj;

    private Transform _player;

    private float _startPos;

    [SerializeField] private Pool[] boundaryPrefab;

    [SerializeField] private int boundarySpawnAmount;

    [Space(15), SerializeField] private Pool[] damageablePrefabs;

    [Space(10), SerializeField] private float yRange;

    [SerializeField] private float zSpacing;

    [Space(10), SerializeField] private int autoDestroyLimit;


    private void Start()
    {
        _player = PlayerController.Instance.PlayerMovement.transform;

        _spawnedPoolObj = GetComponent<PoolObject>();
    }


    private void SpawnObstacle()
    {
        int index = 0;

        int randomNum = Random.Range(1, 4);


        while (index < boundarySpawnAmount)
        {
            float yPos = Random.Range(-yRange, yRange);

            var clone = RandomSpawn(boundaryPrefab,
                new Vector3(0, yPos, transform.position.z + _startPos));

            if (index % randomNum == 0)
                SpawnCollectibles(new Vector3(0, yPos, clone.transform.position.z + 10f));

            index++;

            _startPos += Random.Range(zSpacing, maxInclusive: zSpacing + 5);
        }


        _startPos = 0;
    }


    private void SpawnCollectibles(Vector3 pos)
    {
        var randomNum = Random.Range(0, boundarySpawnAmount);

        switch (randomNum)
        {
            case 0:
                SpawnDiamonds(pos);
                break;
            case 1:
                SpawnCoinMagnet(pos);
                break;
            case 2:
                SpawnGhost(pos);
                break;
            case 3:
                SpawnDamageables(pos);
                break;
            default:
                SpawnCoins(pos);
                break;
        }
    }

    private void SpawnCoins(Vector3 pos)
    {
        var rot = Quaternion.Euler(0f, Random.Range(15f, maxInclusive: 60f), 0f);

        Spawn(0, pos, rot);
    }

    private void SpawnDiamonds(Vector3 pos)
    {
        CloneCollectibles(1, pos);
    }

    private void SpawnCoinMagnet(Vector3 pos)
    {
        if (ItemManager.Instance.CollectibleItem.GetItemByIndex(1).IsUnlocked)
        {
            // LogConsole.Log("Spawned coin magnet!");

            CloneCollectibles(2, pos);
        }
        else
            SpawnCoins(pos);
    }

    private void SpawnGhost(Vector3 pos)
    {
        if (ItemManager.Instance.CollectibleItem.GetItemByIndex(2).IsUnlocked)
        {
            // LogConsole.Log("Spawned ghost!");

            CloneCollectibles(3, pos);
        }
        else
            SpawnCoins(pos);
    }

    private void CloneCollectibles(int index, Vector3 pos)
    {
        Spawn(index, pos);
    }

    private void SpawnDamageables(Vector3 pos)
    {
        switch (Random.Range(0, 2))
        {
            case 1:
                SpawnAsteroid(pos);
                break;
            default:
                SpawnDanger(pos);
                break;
        }
    }

    private void SpawnDanger(Vector3 pos) => Spawn(0, pos, damageablePrefabs);

    private void SpawnAsteroid(Vector3 pos)
    {
        var rot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        Spawn(1, pos, rot, damageablePrefabs);
    }


    private IEnumerator AutoDestroyParent()
    {
        while (true)
        {
            // Despawn nested objects
            if (_player.position.z - transform.position.z > autoDestroyLimit - 10f)
                DespawnItems();

            // Despawn parent
            if (_player.position.z - transform.position.z > autoDestroyLimit)
                _spawnedPoolObj.Despawn();

            yield return Utility.GetWaitForSeconds(1.0f);
        }
    }


    private void DespawnItems()
    {
        Despawn(0);

        Despawn(damageablePrefabs, 0);

        Despawn(boundaryPrefab, 0);
    }


    public void StartAutoDestroy()
    {
        SpawnObstacle();

        if (_player == null)
            _player = PlayerController.Instance.PlayerMovement.transform;

        StartCoroutine(AutoDestroyParent());
    }
}