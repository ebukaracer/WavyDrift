using UnityEngine;


public class ObstacleAnim : MonoBehaviour
{
    Vector3 startPos;

    [SerializeField]
    float delta;

    [SerializeField]
    float speed;


    void Start()
    {
        startPos = transform.localPosition;
    }

    void FixedUpdate()
    {
        Vector3 newPos = startPos;

        newPos.y += delta * Mathf.Sin(Time.time * speed);

        transform.localPosition = new Vector3(transform.localPosition.x, newPos.y, transform.localPosition.z);
    }
}

