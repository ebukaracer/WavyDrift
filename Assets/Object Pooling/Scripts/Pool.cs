using System.Collections.Generic;
using UnityEngine;

namespace Racer.ObjectPooler
{
    [DefaultExecutionOrder(-1)]
    public class Pool : MonoBehaviour
    {
#if UNITY_2021_1_OR_NEWER
        private readonly Queue<PoolObject> _queue = new();
#else
        private readonly Queue<PoolObject> _queue = new Queue<PoolObject>();
#endif
        /* Uncomment if getting the count of spawned active objects is necessary.
         public int ActiveObjects { get; private set; }
        */

        [SerializeField] private int capacity = 5;
        [SerializeField] private bool autoGrow;

        [SerializeField] private PoolObject poolObjectPrefab;

        /* Uncomment if monitoring the events of spawning/despawning is necessary.
        [HideInInspector] public UnityAction<PoolObject> onSpawnObject;
        [HideInInspector] public UnityAction<PoolObject> onDeSpawnObject;
        */

        private void Awake()
        {
            InitializeQueue();
        }

        private void InitializeQueue()
        {
            for (int i = 0; i < capacity; i++)
                InsertObjectToQueue();
        }

        private void InsertObjectToQueue()
        {
            var poolObj = Instantiate(poolObjectPrefab, transform);

            if (poolObj == null)
            {
                Logging.LogWarning(
                    $"No <b>{nameof(PoolObject)} Component</b> was added or inherited <i>{poolObjectPrefab.name}</i> gameobject." +
                    $"\nInherit or add a <b>{nameof(PoolObject)} Component to the gameobject.");

                return;
            }

            poolObj.Pool = this;

            if (poolObj.gameObject.activeInHierarchy)
                poolObj.gameObject.SetActive(false);

            _queue.Enqueue(poolObj);
        }

        // Instantiate object
        public PoolObject SpawnObject() =>
            SpawnObject(poolObjectPrefab.transform.position, poolObjectPrefab.transform.rotation);

        public PoolObject SpawnObject(Vector3 position) => SpawnObject(position, poolObjectPrefab.transform.rotation);

        public PoolObject SpawnObject(Vector3 position, Quaternion rotation)
        {
            if (_queue.Count == 0)
            {
                if (autoGrow)
                {
                    capacity++;
                    InsertObjectToQueue();
                }
                else
                {
                    Logging.LogError(
                        $"[<color=#ff5566><b>Pool out of objects error</b></color>] : No more game objects available in the <i>{name}</i> pool.\n"
                        + "Make sure to increase the <b>Capacity</b> or check the <b>Auto Grow</b> check-box in the inspector.\n\n");

                    return null;
                }
            }

            PoolObject poolObj = _queue.Dequeue();

            poolObj.transform.SetPositionAndRotation(position, rotation);

            if (!poolObj.gameObject.activeInHierarchy)
                poolObj.gameObject.SetActive(true);

            // onSpawnObject?.Invoke(poolObj);

            // ActiveObjects++;

            return poolObj;
        }

        // Destroy object; Return to pool
        public void DespawnObject(PoolObject poolObj)
        {
            if (!poolObj.gameObject.activeInHierarchy)
                return;

            poolObj.gameObject.SetActive(false);

            _queue.Enqueue(poolObj);

            // onDeSpawnObject?.Invoke(poolObj);

            // ActiveObjects--;
        }

        // Destroy object; Return to pool, after some delay.
        public void DespawnObject(PoolObject poolObj, float delay)
        {
            poolObj.InvokeDespawn(delay);
        }
    }
}