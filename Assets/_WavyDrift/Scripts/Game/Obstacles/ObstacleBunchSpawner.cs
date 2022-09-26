using UnityEngine;

internal class ObstacleBunchSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] obstaclePrefabs;

    [Space(10f)]

    // Dynamic
    [SerializeField]
    private float initialSpawnPos = 30f;

    [field: SerializeField]
    public int NextSpawnPos { get; set; }

    [ContextMenu("Spawn Obstacle Bunch")]
    public void SpawnObstacleBunch()
    {
        Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)],
            new Vector3(0, 0, initialSpawnPos),
            Quaternion.identity, transform);

        initialSpawnPos += NextSpawnPos;
    }

    

    [ContextMenu("Clear Obstacle Bunch")]
    private void ClearObstacleBunch()
    {
        initialSpawnPos = 30f;

        if (transform.childCount <= 0)
        {
            Debug.LogWarning($"[{transform.gameObject.name}]  is Empty!");

            return;
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
