using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Racer.LoadManager
{
    // TODO: UnloadSceneAsync
    [DefaultExecutionOrder(-100)]
    public class LoadManager : MonoBehaviour
    {
        public static LoadManager Instance { get; private set; }


        CancellationToken token;

#if UNITY_2021_1_OR_NEWER 
        readonly CancellationTokenSource cancellationToken = new();
#else
        readonly CancellationTokenSource cancellationToken = new CancellationTokenSource();
#endif
        public event Action OnLoadInit;

        public event Action OnLoadStarted;

        public event Action OnLoadFinished;


        public float InitDelay { get; set; }

        int initDelayInMs;


        int delayInMs;

        [SerializeField, Tooltip("Delay in Seconds to last before/after loading, \nNote: This would be ignored if delayBeforeLoad/delayAfterLoad is false")]
        float loadDelay = .5f;


        [SerializeField, Tooltip("Disabling this would activate the scene as soon as loading is completed.")]
        bool delayBeforeLoad = true;

        [SerializeField, Tooltip("Disabling this would activate the loading progress as soon as initialization is complete.")]
        bool delayAfterLoad = true;


        void Awake()
        {
            if (Instance == null)
                Instance = this;

            // Time(ms) to elapse while loading.
            // Without this, loading would be instant depending on the scenes' complexity.
            delayInMs = (int)(loadDelay * 1000);

            token = cancellationToken.Token;
        }


        /// <summary>
        /// Initializes and loads the next scene asynchronously with a little delay.
        /// </summary>
        /// <param name="sceneIndex">The next Scene-index to load</param>
        public async void LoadSceneAsync(int sceneIndex, LoadSceneMode loadSceneMode = default)
        {
            // Notifies listeners about initialization commencement.
            OnLoadInit?.Invoke();

            // Initialization delay time in ms.
            initDelayInMs = (int)(InitDelay * 1000);

            // Few delays before starting to load.
            // UI fade-in/out effect time is applicable here.
            await Task.Delay(initDelayInMs, token);

            // Few delays(ms) before loading is started.
            if (delayBeforeLoad)
                await Task.Delay(delayInMs, token);

            var scene = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
            
            scene.allowSceneActivation = false;

            // Notifies listeners that loading has started.
            OnLoadStarted?.Invoke();

            // Notifies listeners on current loading progress
            // Applicable to UI-fills
            do
            {
                // 1sec delay while scene-loading is in progress.
                await Task.Delay(1000, token);
            }
            while
                (scene.progress < .9f);


            if (scene.progress >= .9f)
            {
                OnLoadFinished?.Invoke();

                // Few delays(ms) after loading is completed.
                if (delayAfterLoad)
                    await Task.Delay(delayInMs, token);

                // TODO: try to fade-out UI-elements after this has been set to true.
                scene.allowSceneActivation = true;
            }
        }


        // Clean ups
        private void OnDisable()
        {
            cancellationToken?.Cancel();

            cancellationToken?.Dispose();
        }
    }
}