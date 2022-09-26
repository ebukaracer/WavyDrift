using UnityEngine;

namespace Racer.LoadManager
{
    /// <summary>
    /// Simulates loading circle.
    /// Automatically starts simulating when the containing gameobject is active.
    /// Containing gameobject must be inactive for this to work appropriately.
    /// </summary>
    internal class LoadingCircle : MonoBehaviour
    {
        private RectTransform _mainIcon;

        private Vector3 _iconAngle;

        private float _startTime;

        [SerializeField] private float timeStep = 0.03f;

        [SerializeField] private float oneStepAngle = 30;



        private void Awake()
        {
            _mainIcon = GetComponent<RectTransform>();

            _startTime = Time.time;
        }

        private void Start()
        {
            LoadManager.Instance.OnLoadFinished += Instance_OnLoadFinished;
        }


        private void Update()
        {
            // TODO: Find a smoother way to interpolate the rotation.
            if (!(Time.time - _startTime >= timeStep)) return;

            _iconAngle = _mainIcon.localEulerAngles;

            _iconAngle.z += oneStepAngle;

            _mainIcon.localEulerAngles = _iconAngle;

            _startTime = Time.time;
        }

        private void Instance_OnLoadFinished()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _startTime = 0;
        }
    }
}