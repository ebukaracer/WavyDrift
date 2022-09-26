using Racer.ObjectPooler;
using UnityEngine;

public abstract class MultipleSpawner : MonoBehaviour
{
    [field: SerializeField]
    public Pool[] Pools { get; private set; }

    protected PoolObject[] poolObjects;



    protected virtual PoolObject Spawn(int index, Vector3 pos, Quaternion rot, Pool[] pools = default)
    {
        if (pools != default)
            return pools[index].SpawnObject(
           pos,
           rot);

        return Pools[index].SpawnObject(
           pos,
           rot);
    }

    protected virtual PoolObject Spawn(int index, Pool[] pools = default)
    {
        if (pools != default)
            return pools[index].SpawnObject();

        return Pools[index].SpawnObject();
    }

    protected virtual PoolObject Spawn(int index, Vector3 pos, Pool[] pools = default)
    {
        if (pools != default)
            return pools[index].SpawnObject(pos);

        return Pools[index].SpawnObject(pos);
    }



    protected virtual PoolObject RandomSpawn(Pool[] pools, Vector3 pos, Quaternion rot)
    {
        var poolArr = pools[Random.Range(0, pools.Length)];

        return poolArr.SpawnObject(
           pos,
           rot
        );
    }

    protected virtual PoolObject RandomSpawn(Pool[] pools)
    {
        var poolArr = pools[Random.Range(0, pools.Length)];

        return poolArr.SpawnObject();
    }

    protected virtual PoolObject RandomSpawn(Pool[] pools, Vector3 pos)
    {
        var poolArr = pools[Random.Range(0, pools.Length)];

        return poolArr.SpawnObject(pos);
    }

    protected virtual void Despawn(float delay)
    {
        for (int i = 0; i < Pools.Length; i++)
        {
            poolObjects = Pools[i].GetComponentsInChildren<PoolObject>();

            for (int j = 0; j < poolObjects.Length; j++)
            {
                Pools[i].DespawnObject(poolObjects[j], delay);
            }
        }
    }

    protected virtual void Despawn(Pool[] pools, float delay)
    {
        for (int i = 0; i < pools.Length; i++)
        {
            poolObjects = pools[i].GetComponentsInChildren<PoolObject>();

            for (int j = 0; j < poolObjects.Length; j++)
            {
                pools[i].DespawnObject(poolObjects[j], delay);
            }
        }
    }


    #region TODO_Spawn via string

    /*
    public virtual PoolObject Spawn(string prefabName, Vector3 pos, Quaternion rot)
    {
        var poolObj = pools.Where(n => n.poolObjectPrefab.name.ToLower() == prefabName.ToLower()).FirstOrDefault();

        if (poolObj == null)
        {
            LogConsole.LogWarning($"[{prefabName}] does not exist in the pool!");

            return null;
        }

        return spawnedObj = poolObj.SpawnObject(
           pos,
           rot
        );
    }
    */
    #endregion

    #region TODD_Despawn via string
    /*
    public virtual void Despawn(string prefabName, float delay)
    {
        var poolObj = pools.Where(n => n.poolObjectPrefab.name.ToLower() == prefabName.ToLower()).FirstOrDefault();

        if (poolObj == null)
        {
            LogConsole.LogWarning($"[{prefabName}] does not exist in the pool!");

            return;
        }

        poolObj.DespawnObject(spawnedObj, delay);
    }
    */
    #endregion

    // This is subtle in the case of despawning in the same class where you spawned.
    // public virtual void Despawn(int index, float delay) => Pools[index].DespawnObject(spawnedObj, delay);

}



