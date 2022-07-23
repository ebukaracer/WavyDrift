using UnityEngine;

namespace Racer.LoadManager
{
    /// <summary>
    /// Simulates loading circle.
    /// Automatically starts simulating when the containing gameobject is active.
    /// Containing gameobject must be inactive for this to work appropriately.
    /// </summary>
    class LoadingCircle : MonoBehaviour
    {
        RectTransform _mainIcon;

        Vector3 iconAngle;

        float _startTime;

        [SerializeField] float _timeStep = 0.03f;

        [SerializeField] float _oneStepAngle = 30;



        private void Awake()
        {
            _mainIcon = GetComponent<RectTransform>();

            _startTime = Time.time;
        }

        private void Start()
        {
            LoadManager.Instance.OnLoadFinished += Instance_OnLoadFinished;
        }


        void Update()
        {
            // TODO: Find a smoother way to interpolate the rotation.
            if (Time.time - _startTime >= _timeStep)
            {
                iconAngle = _mainIcon.localEulerAngles;

                iconAngle.z += _oneStepAngle;

                _mainIcon.localEulerAngles = iconAngle;

                _startTime = Time.time;
            }
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