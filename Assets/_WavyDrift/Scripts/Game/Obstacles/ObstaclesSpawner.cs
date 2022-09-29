using System.Collections;
using System.Linq;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    private Transform _player;

    private int _startOffset;
    private int _index;
    private int _randomNum;

    [SerializeField] private Transform[] boundaryPrefab;
    [SerializeField] private Transform[] damageablesPrefab;
    [SerializeField] private Transform[] collectiblesPrefab;

    [Space(5), Header("PLATFORM PROPERTIES")]
    [SerializeField] private int boundarySpawnAmount;
    [SerializeField] private float yRange = 10;
    [SerializeField] private int zSpacing = 15;
    [SerializeField] private int autoDestroyLimit = 300;

    [Space(5), SerializeField] private bool spawnCollectibles = true;


    private void Start()
    {
        RandomizeOnStart();

        _player = PlayerController.Instance.PlayerMovement.transform;

        StartCoroutine(AutoDestroyMe());
    }


    [ContextMenu("Spawn Obstacles")]
    private void SpawnObstacle()
    {
        _index = 0;

        _randomNum = Random.Range(1, 4);


        while (_index < boundarySpawnAmount)
        {
            var randomBoundary = boundaryPrefab[Random.Range(0, boundaryPrefab.Length)];

            var yPos = Random.Range(-yRange, yRange);

            var clone = Instantiate(randomBoundary,
                new Vector3(0, yPos, transform.position.z + _startOffset),
                randomBoundary.rotation,
                transform);

            if (_index % _randomNum == 0)
                SpawnCollectibles(new Vector3(0, yPos, clone.position.z + 10f));

            _index++;

            // 15 -> 20
            _startOffset += Random.Range(zSpacing, zSpacing + 6);
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

    private void SpawnCoins(Vector3 pos) =>
        Clone(0, pos);

    private void SpawnDiamonds(Vector3 pos)
    {
        Clone(1, pos);
    }

    private void SpawnCoinMagnet(Vector3 pos)
    {
        if (!Application.isPlaying || !spawnCollectibles)
            return;

        if (ItemManager.Instance.CollectibleItem.GetItemByIndex(1).IsUnlocked)
            Clone(2, pos);
    }

    private void SpawnGhost(Vector3 pos)
    {
        if (!Application.isPlaying || !spawnCollectibles)
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

    private void SpawnDamageables(Vector3 pos)
    {
        int randomItem = Random.Range(0, damageablesPrefab.Length);

        Instantiate(damageablesPrefab[randomItem],
                            pos,
                            collectiblesPrefab[randomItem].rotation,
                            transform);
    }


    private void RandomizeOnStart()
    {
        if (Random.Range(0, 2) == 1)
            RefreshObstacle();
    }

    [ContextMenu("Clear Obstacles")]
    private void ClearObstacles()
    {
        // TODO: Review
        var children = transform.GetComponentsInChildren<Obstacles>().Where(c => c.gameObject.name.Contains("Clone"));

        var obstaclesEnumerable = children.ToList();

        if (!obstaclesEnumerable.Any())
        {
            Logging.LogWarning(($"[{transform.gameObject.name}]  is Empty!"));

            return;
        }

        foreach (var child in obstaclesEnumerable)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
    }

    [ContextMenu("Refresh Obstacles")]
    private void RefreshObstacle()
    {
        ClearObstacles();

        SpawnObstacle();
    }

    private IEnumerator AutoDestroyMe()
    {
        while (true)
        {
            if (_player.position.z - transform.position.z > autoDestroyLimit)
            {
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}