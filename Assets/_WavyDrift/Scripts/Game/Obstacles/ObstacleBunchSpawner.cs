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
        _maxSpawnCount = Pools.Length - 2;

        if (ItemManager.Instance.PlayerItem.GetItemByName(PlayerName.Jet).IsPurchased)
        {
            _maxSpawnCount = Pools.Length - 1;
        }
        if (ItemManager.Instance.PlayerItem.GetItemByName(PlayerName.Jetpackboy).IsPurchased)
        {
            _maxSpawnCount = Pools.Length;
        }

    }
    public void PoolObstacleBunch()
    {
        var poolObj = Spawn(Random.Range(0, _maxSpawnCount),
            new Vector3(0, 0, initialSpawnPos),
            Quaternion.identity);

        poolObj.GetComponent<ObstaclesSpawner>().StartAutoDestroy();

        initialSpawnPos += NextSpawnPos;
    }
}
