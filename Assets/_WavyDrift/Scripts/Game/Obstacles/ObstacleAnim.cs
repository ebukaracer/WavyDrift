using UnityEngine;


internal class ObstacleAnim : MonoBehaviour
{
    private Vector3 _startPos;

    [SerializeField] private float delta;

    [SerializeField] private float speed;


    private void Start()
    {
        _startPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        var newPos = _startPos;

        newPos.y += delta * Mathf.Sin(Time.time * speed);

        transform.localPosition = new Vector3(transform.localPosition.x, newPos.y, transform.localPosition.z);
    }
}

