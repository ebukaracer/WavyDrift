using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// Developed by : Hamza Herbou
/// ------------------------------------
/// Email  : hamza95herbou@gmail.com
/// GITHUB : https://github.com/herbou/

namespace GDTools.ObjectPooling
{

    public class Pool : MonoBehaviour
    {
        [SerializeField] private int capacity = 5;
        [SerializeField] private bool autoGrow = false;

        [SerializeField] private GameObject poolObjectPrefab;

        public int ActiveObjects { get; private set; } = 0;

        [HideInInspector] public UnityAction<PoolObject> OnSpawnObject;
        [HideInInspector] public UnityAction<PoolObject> OnDespawnObject;

        readonly private Queue<PoolObject> queue = new Queue<PoolObject>();


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
            var poolObj = Instantiate(poolObjectPrefab, transform).GetComponent<PoolObject>();

            if (poolObj == null)
            {
                Debug.LogWarning($"No <b>{nameof(PoolObject)} Component</b> was added or inherited <i>{poolObjectPrefab.name}</i> gameobject." +
                    $"\nInherit or add a <b>{nameof(PoolObject)} Component to the gameobject.");

                return;
            }

            // poolObj.transform.SetPositionAndRotation(poolObjectPrefab.transform.position, poolObjectPrefab.transform.rotation);

            poolObj.Pool = this;

            if (poolObj.gameObject.activeInHierarchy)
                poolObj.gameObject.SetActive(false);

            queue.Enqueue(poolObj);
        }



        // Instantiate object : ------------------------------------------------
        public PoolObject SpawnObject() => SpawnObject(poolObjectPrefab.transform.position, poolObjectPrefab.transform.rotation);

        public PoolObject SpawnObject(Vector3 position) => SpawnObject(position, poolObjectPrefab.transform.rotation);

        public PoolObject SpawnObject(Vector3 position, Quaternion rotation)
        {
            if (queue.Count == 0)
            {
                if (autoGrow)
                {
                    capacity++;
                    InsertObjectToQueue();
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError(@"[ <color=#ff5566><b>Pool out of objects error</b></color> ] : No more game objects available in the <i>" + name + "</i> pool.\n"
                    + "Make sure to increase the <b>Capacity</b> or check the <b>Auto Grow</b> check-box in the inspector.\n\n", gameObject);
#endif
                    return null;
                }
            }

            PoolObject poolObj = queue.Dequeue();

            poolObj.transform.SetPositionAndRotation(position, rotation);

            if (!poolObj.gameObject.activeInHierarchy)
                poolObj.gameObject.SetActive(true);

            OnSpawnObject?.Invoke(poolObj);

            ActiveObjects++;

            return poolObj;
        }



        // Destroy object : ------------------------------------------------
        public void DespawnObject(PoolObject poolObj)
        {
            if (!poolObj.gameObject.activeInHierarchy)
                return;

            poolObj.gameObject.SetActive(false);

            queue.Enqueue(poolObj);

            OnDespawnObject?.Invoke(poolObj);

            ActiveObjects--;
        }

        public void DespawnObject(PoolObject poolObj, float delay)
        {
            poolObj.InvokeDespawn(delay);
        }
    }
}