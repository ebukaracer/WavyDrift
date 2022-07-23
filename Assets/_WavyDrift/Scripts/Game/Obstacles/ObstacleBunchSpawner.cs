using UnityEngine;

class ObstacleBunchSpawner : MultipleSpawner
{
    int maxSpawnCount;

    // Dynamic
    [SerializeField]
    float initialSpawnPos = 30f;

    [field: SerializeField]
    public int NextSpawnPos { get; set; }

    private void Start()
    {
        maxSpawnCount = Pools.Length - 2;

        if (ItemManager.Instance.PlayerItem.GetItemByName(PlayerName.Jet).IsPurchased)
        {
            maxSpawnCount = Pools.Length - 1;
        }
        if (ItemManager.Instance.PlayerItem.GetItemByName(PlayerName.Jetpackboy).IsPurchased)
        {
            maxSpawnCount = Pools.Length;
        }

    }
    public void PoolObstacleBunch()
    {
        var poolObj = Spawn(Random.Range(0, maxSpawnCount),
            new Vector3(0, 0, initialSpawnPos),
            Quaternion.identity);

        poolObj.GetComponent<ObstaclesSpawner>().StartAutoDestroy();

        initialSpawnPos += NextSpawnPos;
    }
}
