using UnityEngine;
using GDTools.ObjectPooling;

[RequireComponent(typeof(Pool))]
abstract class Spawner : MonoBehaviour
{
    protected Pool pool;

    protected PoolObject spawnedObj;

    protected virtual void Awake() => pool = GetComponent<Pool>();

    public virtual PoolObject Spawn(Vector3 pos, Quaternion rot)
    {
        return spawnedObj = pool.SpawnObject(
           pos,
           rot
        );
    }

    public virtual void Despawn(float delay) => pool.DespawnObject(spawnedObj, delay);

}

[RequireComponent(typeof(Pool))]
abstract class Spawner<T> : MonoBehaviour where T : PoolObject
{
    protected Pool pool;

    protected T spawnedObj;

    protected virtual void Awake() => pool = GetComponent<Pool>();

    public virtual T Spawn(Vector3 pos, Quaternion rot)
    {
        return spawnedObj = (T)pool.SpawnObject(
            pos,
            rot
            );
    }

    public virtual void Despawn(float delay) => pool.DespawnObject(spawnedObj, delay);
}



