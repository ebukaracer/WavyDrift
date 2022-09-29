using UnityEngine;

internal class ObstacleAnim : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _currentPos;

    private bool _isGameover;

    [Header("MOTION")]
    [SerializeField] private float delta;
    [SerializeField] private float speed;


    private void Start()
    {
        _startPos = transform.localPosition;
        _currentPos = _startPos;

        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    private void GameManager_OnCurrentState(GameStates state)
    {
        _isGameover = state.Equals(GameStates.GameOver);
    }

    private void FixedUpdate()
    {
        if (_isGameover)
            return;

        _currentPos = transform.localPosition;

        var newPos = _startPos;

        newPos.y += delta * Mathf.Sin(Time.time * speed);

        _currentPos = new Vector3(_currentPos.x, newPos.y, _currentPos.z);

        transform.localPosition = _currentPos;
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}

