using Racer.SaveSystem;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    Transform player;

    float startPos;

    [SerializeField]
    Transform[] boundaryPrefab;

    [SerializeField]
    int boundarySpawnAmount;

    [Space(10)]

    [SerializeField]
    Transform[] collectiblesPrefab;

    [SerializeField]
    bool spawnCollectibles = true;

    [Space(15)]

    [SerializeField]
    Transform[] damageablesPrefab;

    [Space(10)]

    [SerializeField]
    float yRange;

    [SerializeField]
    float zSpacing;
    [SerializeField]

    [Space(10)]

    int autoDestroyLimit;


    private void Start()
    {
        RandomizeOnStart();

        player = PlayerController.Instance.PlayerMovement.transform;

        StartCoroutine(AutoDestroyMe());
    }


    [ContextMenu("Spawn Obstacles")]
    void SpawnObstacle()
    {
        int index = 0;

        int randomNum = Random.Range(1, 4);


        while (index < boundarySpawnAmount)
        {
            var randomBoundary = boundaryPrefab[Random.Range(0, boundaryPrefab.Length)];

            float yPos = Random.Range(-yRange, yRange);

            var clone = Instantiate(randomBoundary,
                new Vector3(0, yPos, transform.position.z + startPos),
                randomBoundary.rotation,
                transform);

            if (index % randomNum == 0)
                SpawnCollectibles(new Vector3(0, yPos, clone.position.z + 10f));

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

    private void SpawnCoins(Vector3 pos) =>
        Clone(0, pos);

    private void SpawnDiamonds(Vector3 pos)
    {
        Clone(1, pos);
    }

    private void SpawnCoinMagnet(Vector3 pos)
    {
        if (Application.isEditor || !spawnCollectibles)
            return;

        if (ItemManager.Instance.CollectibleItem.GetItemByIndex(1).IsUnlocked)
            Clone(2, pos);
    }

    private void SpawnGhost(Vector3 pos)
    {
        if (Application.isEditor || !spawnCollectibles)
            return;

        if (ItemManager.Instance.CollectibleItem.GetItemByIndex(2).IsUnlocked)
            Clone(3, pos);
    }

    private void Clone(int index, Vector3 pos)
    {
        Instantiate(collectiblesPrefab[index],
                            pos,
                            collectiblesPrefab[index].rotation,
                            transform);
    }

    void SpawnDamageables(Vector3 pos)
    {
        int randomItem = Random.Range(0, damageablesPrefab.Length);

        Instantiate(damageablesPrefab[randomItem],
                            pos,
                            collectiblesPrefab[randomItem].rotation,
                            transform);
    }


    void RandomizeOnStart()
    {
        if (Random.Range(0, 2) == 1)
            RefreshObstacle();
    }

    [ContextMenu("Clear Obstacles")]
    void ClearObstacles()
    {
        var children = transform.GetComponentsInChildren<Obstacles>().Where(c => c.gameObject.name.Contains("Clone"));

        if (children.Count() <= 0)
        {
            Debug.LogWarning(($"[{transform.gameObject.name}]  is Empty!"));

            return;
        }

        foreach (var child in children)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
    }

    [ContextMenu("Refresh Obstacles")]
    void RefreshObstacle()
    {
        ClearObstacles();

        SpawnObstacle();
    }

    IEnumerator AutoDestroyMe()
    {
        while (true)
        {
            if (player.position.z - transform.position.z > autoDestroyLimit)
            {
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
