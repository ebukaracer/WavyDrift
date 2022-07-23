using UnityEngine;

namespace GDTools.ObjectPooling
{
    public class PoolObject : MonoBehaviour
    {
        Pool pool;

        // <summary>
        /// Initialized once in <see cref="Pool.InsertObjectToQueue"/>.
        /// Prevents multiple initialization from other scripts.
        /// </summary>
        public Pool Pool
        {
            get => pool;

            set
            {
                if (pool == null)
                    pool = value;
                else
                    Debug.LogWarning($"Value [{nameof(pool)}] has already been assigned to, Ignoring...");
            }
        }

        public virtual void InvokeDespawn(float delay)
        {
            Invoke(nameof(Despawn), delay);
        }

        public virtual void Despawn()
        {
            pool.DespawnObject(this);
        }
    }
}