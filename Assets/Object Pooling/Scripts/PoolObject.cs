using UnityEngine;

namespace Racer.ObjectPooler
{
    public class PoolObject : MonoBehaviour
    {
        private Pool _pool;

        /// <summary>
        /// Initialized once in <see cref="Pool.InsertObjectToQueue"/>.
        /// Prevents multiple initialization from other scripts.
        /// </summary>
        public Pool Pool
        {
            get => _pool;

            set
            {
                if (_pool == null)
                    _pool = value;
                else
                    Logging.LogWarning($"Value [{nameof(_pool)}] has already been assigned to, Ignoring...");
            }
        }

        /// <summary>
        /// De-spawns this object after a delay.
        /// The De-spawned object is returned to the pool.
        /// </summary>
        /// <param name="delay">Time to elapse.</param>
        public virtual void InvokeDespawn(float delay)
        {
            Invoke(nameof(Despawn), delay);
        }

        /// <summary>
        /// De-spawns this object.
        /// See also: <seealso cref="InvokeDespawn"/>
        /// </summary>
        public virtual void Despawn()
        {
            _pool.DespawnObject(this);
        }
    }
}