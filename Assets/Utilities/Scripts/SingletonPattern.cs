using UnityEngine;

namespace Racer.Utilities
{
    public sealed class SingletonPattern
    {
        SingletonPattern() { }

        /// <summary>
        /// A static instance is similar to a singleton, but instead of destroying any new instances,
        /// it overrides the current instance. This is handy for resetting the state.
        /// </summary>
        public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
        {
            public static T Instance { get; private set; }

            protected virtual void Awake() => Instance = this as T;


            private void OnApplicationQuit()
            {
                Instance = null;

                Destroy(gameObject);
            }
        }

        /// <summary>
        /// This transforms the static instance into a basic singleton. This will destroy any new 
        /// version created leaving the original intact.
        /// </summary>
        public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
        {
            protected override void Awake()
            {
                if (Instance != null)
                    Destroy(gameObject);
                else
                    base.Awake();
            }
        }

        /// <summary>
        /// This will persist/survive through scene loads. 
        /// </summary>
        public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
        {
            protected override void Awake()
            {
                base.Awake();

                DontDestroyOnLoad(gameObject);
            }
        }
    }
}