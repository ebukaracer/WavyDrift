using System;
using System.Collections;
using Racer.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Racer.LoadManager
{
    // TODO: UnloadSceneAsync
    [DefaultExecutionOrder(-100)]
    public class LoadManager : SingletonPattern.Singleton<LoadManager>
    {
        private WaitForSeconds _waitLoadTimeDelay;
        private WaitForSeconds _waitInitDelay;
        private WaitForSeconds _waitLoadDelay;

        public event Action OnLoadInit;
        public event Action OnLoadStarted;
        public event Action OnLoadFinished;

        // Prevents loading more than once.
        private bool _isLoading;

        // This is the delay for UI elements(eg loading_bar) to be initialized.
        public float InitDelay { get; set; }

        [SerializeField, Tooltip("Little time to spend while Loading.\n This is useful for less-detailed scenes.")]
        private float loadTimeDelay = 1f;

        [Space(5), SerializeField, Tooltip("Delay in Seconds to last before/after loading. \nNote: This would be ignored if delayBeforeLoad and delayAfterLoad is false")]
        private float loadDelay = 1f;

        [SerializeField, Tooltip("Disabling this would activate the scene as soon as loading is completed.")]
        private bool delayBeforeLoad = true;

        [SerializeField, Tooltip("Disabling this would activate the loading progress as soon as initialization is complete.")]
        private bool delayAfterLoad = true;


        protected override void Awake()
        {
            base.Awake();

            _waitLoadDelay = new WaitForSeconds(loadDelay);

            // Time(s) to elapse while loading.
            // Without this, loading would be instant depending on the scenes' complexity.
            _waitLoadTimeDelay = new WaitForSeconds(loadTimeDelay);

            // Time(s) to elapse before loading is started.
            // This time is required to setup necessary stuffs before loading commencement.
            _waitInitDelay = new WaitForSeconds(InitDelay);
        }

        // Todo: Support for additional overloads for locating scene.
        public void LoadSceneAsync(int sceneIndex, LoadSceneMode mode = default)
        {
            if (_isLoading)
                return;

            StartCoroutine(LoadScene(sceneIndex, mode));
        }

        /// <summary>
        /// Loads Asynchronously using co-routine
        /// </summary>
        private IEnumerator LoadScene(int sceneIndex, LoadSceneMode loadSceneMode = default)
        {
            _isLoading = true;

            // Notifies listeners about initialization commencement.
            OnLoadInit?.Invoke();

            // Initialization delay.
            // Few delays before starting to load.
            // UI fade-in/out effect time is applicable here.
            yield return _waitInitDelay;

            // Few delays(ms) before loading is started.
            if (delayBeforeLoad)
                yield return _waitLoadDelay;

            var scene = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);

            scene.allowSceneActivation = false;

            // Notifies listeners that loading has started.
            OnLoadStarted?.Invoke();

            // Notifies listeners on current loading progress
            // Applicable to UI-fills
            do
            {
                // Little delay while scene-loading is in progress.
                // If loading seems faster(less-detailed scenes), increase this delay.
                yield return _waitLoadTimeDelay;
            }
            while
                (scene.progress < .9f);


            if (!(scene.progress >= .9f)) yield break;
            OnLoadFinished?.Invoke();

            // Few delays(ms) after loading is completed.
            if (delayAfterLoad)
                yield return _waitLoadDelay;

            // TODO: try to fade-out UI-elements after this has been set to true.
            scene.allowSceneActivation = true;

            _isLoading = false;
        }
    }
}