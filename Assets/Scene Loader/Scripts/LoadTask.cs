using UnityEngine;

namespace Racer.LoadManager
{
    /// <summary>
    /// Create your custom loader by inheriting from this class, 
    /// Override <see cref="EnableLoaderDefaultAnimation"/>,
    /// Then in <see cref="AnimatorTask"/> or <see cref="CanvasGroupTask"/>, 
    /// Inherit from the custom class/es you created.
    /// </summary>
    internal abstract class LoadTask : MonoBehaviour
    {
        [Tooltip("The object to simulate the loading process.\nExample: A loading circle")]
        public GameObject loaderObject;

        public bool fadeOnStart;

        /// <summary>
        /// Default loader; enables the loader gameobject at a specified interval.
        /// Provide your custom loader by overriding this method.
        /// </summary>
        protected virtual void EnableLoaderDefaultAnimation()
        {
            if (loaderObject != null && !loaderObject.activeInHierarchy)
                loaderObject.SetActive(true);
        }
    }
}