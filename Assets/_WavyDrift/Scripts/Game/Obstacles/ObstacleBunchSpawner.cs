using UnityEngine;

internal class ObstacleBunchSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] obstaclePrefabs;

    // Dynamic field
    [Space(10f), SerializeField]
    private int initialSpawnPos = 30;

    [field: SerializeField] public int NextSpawnPos { get; set; } = 260;


    // Editor/Play Mode
    [ContextMenu("Spawn Obstacle Bunch")]
    public void SpawnObstacleBunch()
    {
        Instantiate(obstaclePrefabs[Random.Range(0, GetSpawnCount())],
            new Vector3(0, 0, initialSpawnPos),
            Quaternion.identity, transform);

        initialSpawnPos += NextSpawnPos;
    }

    // Editor Mode
    [ContextMenu("Clear Obstacle Bunch")]
    private void ClearObstacleBunch()
    {
        if (transform.childCount <= 0)
        {
            initialSpawnPos = 30;

            Logging.LogWarning($"[{transform.gameObject.name}]  is Empty!");

            return;
        }

        for (int i = transform.childCount - 1; i >= 0;)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);

            initialSpawnPos -= NextSpawnPos;

            break;
        }
    }

    private int GetSpawnCount()
    {
        if (!Application.isPlaying)
            return obstaclePrefabs.Length - 2;

        if (ItemManager.Instance.PlayerItem.GetItemByName(PlayerName.Jetpackboy).IsPurchased)
            return obstaclePrefabs.Length;

        if (ItemManager.Instance.PlayerItem.GetItemByName(PlayerName.Jet).IsPurchased)
            return obstaclePrefabs.Length - 1;

        return obstaclePrefabs.Length - 2;
    }
}
