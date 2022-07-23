using GDTools.ObjectPooling;
using Racer.Utilities;
using System.Collections;
using UnityEngine;

public class ObstaclesSpawner : MultipleSpawner
{
    PoolObject spawnedPoolObj;

    Transform player;

    float startPos;

    [SerializeField]
    Pool[] boundaryPrefab;

    [SerializeField]
    int boundarySpawnAmount;


    [Space(15)]

    [SerializeField]
    Pool[] damageablesPrefab;

    [Space(10)]

    [SerializeField]
    float yRange;

    [SerializeField]
    float zSpacing;

    [Space(10)]

    [SerializeField]
    int autoDestroyLimit;


    private void Start()
    {
        player = PlayerController.Instance.PlayerMovement.transform;

        spawnedPoolObj = GetComponent<PoolObject>();
    }


    void SpawnObstacle()
    {
        int index = 0;

        int randomNum = Random.Range(1, 5);


        while (index < boundarySpawnAmount)
        {
            float yPos = Random.Range(-yRange, yRange);

            var clone = RandomSpawn(boundaryPrefab,
                new Vector3(0, yPos, transform.position.z + startPos));

            if (index % randomNum == 0)
                SpawnCollectibles(new Vector3(0, yPos, clone.transform.position.z + 10f));

            index++;

            startPos += Random.Range(zSpacing, zSpacing + 5);
        }


        startPos = 0;
    }



    void SpawnCollectibles(Vector3 pos)
    {
        var randomNum = Random.Range(0, boundarySpawnAmount);

        if (randomNum == 0)
            SpawnDiamonds(pos);

        else if (randomNum == 1)
            SpawnCoinMagnet(pos);

        else if (randomNum == 2)
            SpawnGhost(pos);

        else if (randomNum == 3)
            SpawnDamageables(pos);

        else
            SpawnCoins(pos);
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

    void SpawnDamageables(Vector3 pos)
    {
        switch (Random.Range(0, 2))
        {
            case 1:
                SpawnAsteriod(pos);
                break;
            default:
                SpawnDanger(pos);
                break;
        }
    }

    void SpawnDanger(Vector3 pos) => Spawn(0, pos, damageablesPrefab);

    void SpawnAsteriod(Vector3 pos)
    {
        var rot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        Spawn(1, pos, rot, damageablesPrefab);
    }



    IEnumerator AutoDestroyParent()
    {
        while (true)
        {
            // Despawn nested objects
            if (player.position.z - transform.position.z > autoDestroyLimit - 10f)
                DespawnItems();

            // Despawn parent
            if (player.position.z - transform.position.z > autoDestroyLimit)
                spawnedPoolObj.Despawn();

            yield return Utility.GetWaitForSeconds(1.0f);
        }
    }


    void DespawnItems()
    {
        Despawn(0);

        Despawn(damageablesPrefab, 0);

        Despawn(boundaryPrefab, 0);
    }


    public void StartAutoDestroy()
    {
        SpawnObstacle();

        if (player == null)
            player = PlayerController.Instance.PlayerMovement.transform;

        StartCoroutine(AutoDestroyParent());
    }
}