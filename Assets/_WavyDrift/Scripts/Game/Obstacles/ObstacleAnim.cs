using UnityEngine;


internal class ObstacleAnim : MonoBehaviour
{
    private bool _isGameover;
    private Vector3 _startPos;

    [SerializeField] private float delta;
    [SerializeField] private float speed;


    private void Start()
    {
        _startPos = transform.localPosition;

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

        var newPos = _startPos;

        newPos.y += delta * Mathf.Sin(Time.time * speed);

        transform.localPosition = new Vector3(transform.localPosition.x, newPos.y, transform.localPosition.z);
    }

    private void OnDestroy()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}

