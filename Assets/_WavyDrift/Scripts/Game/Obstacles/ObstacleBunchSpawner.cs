using UnityEngine;

internal class ObstacleBunchSpawner : MultipleSpawner
{
    private int _maxSpawnCount;

    // Dynamic
    [SerializeField] private float initialSpawnPos = 30f;

    [field: SerializeField]
    public int NextSpawnPos { get; set; }

    private void Start()
    {
        _maxSpawnCount = GetSpawnCount();
    }

    public void PoolObstacleBunch()
    {
        var poolObj = Spawn(Random.Range(0, _maxSpawnCount),
            new Vector3(0, 0, initialSpawnPos),
            Quaternion.identity);

        poolObj.GetComponent<ObstaclesSpawner>().StartAutoDestroy();

        initialSpawnPos += NextSpawnPos;
    }

    private int GetSpawnCount()
    {
        if (ItemManager.Instance.PlayerItem.GetItemByName(PlayerName.Jetpackboy).IsPurchased)
            return Pools.Length;

        if (ItemManager.Instance.PlayerItem.GetItemByName(PlayerName.Jet).IsPurchased)
            return Pools.Length - 1;

        return Pools.Length - 2;
    }
}
