using Racer.Utilities;
using System.Collections;
using Racer.ObjectPooler;
using UnityEngine;

internal class ObstaclesSpawner : MultipleSpawner
{
    private int _startOffset;
    private int _index;
    private int _randomNum;

    private PoolObject _spawnedPoolObj;
    private Transform _player;

    [SerializeField] private Pool[] damageablePrefabs;
    [SerializeField] private Pool[] boundaryPrefabs;

    [Space(5), Header("PLATFORM PROPERTIES")]
    [SerializeField] private int boundarySpawnAmount = 15;
    [SerializeField] private float yRange = 10;
    [SerializeField] private int zSpacing = 15;
    [SerializeField] private int autoDestroyLimit = 300;


    private void Start()
    {
        _player = PlayerController.Instance.PlayerMovement.transform;

        _spawnedPoolObj = GetComponent<PoolObject>();
    }


    private void SpawnObstacle()
    {
        _index = 0;

        _randomNum = Random.Range(1, 4);

        while (_index < boundarySpawnAmount)
        {
            var yPos = Random.Range(-yRange, yRange);

            var clone = RandomSpawn(boundaryPrefabs,
                new Vector3(0, yPos, transform.position.z + _startOffset));

            if (_index % _randomNum == 0)
                SpawnCollectibles(new Vector3(0, yPos, clone.transform.position.z + 10f));

            _index++;

            _startOffset += Random.Range(zSpacing, zSpacing + 5);
        }

        _startOffset = 0;
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
        var rot = Quaternion.Euler(0f, Random.Range(15f, 60f), 0f);

        Spawn(0, pos, rot);
    }

    private void SpawnDiamonds(Vector3 pos)
    {
        CloneCollectibles(1, pos);
    }

    private void SpawnCoinMagnet(Vector3 pos)
    {
        if (ItemManager.Instance.CollectibleItem.GetItemByIndex(1).IsUnlocked)
            CloneCollectibles(2, pos);
        else
            SpawnCoins(pos);
    }

    private void SpawnGhost(Vector3 pos)
    {
        if (ItemManager.Instance.CollectibleItem.GetItemByIndex(2).IsUnlocked)
            CloneCollectibles(3, pos);
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

        Despawn(boundaryPrefabs, 0);
    }


    public void StartAutoDestroy()
    {
        SpawnObstacle();

        if (_player == null)
            _player = PlayerController.Instance.PlayerMovement.transform;

        StartCoroutine(AutoDestroyParent());
    }
}