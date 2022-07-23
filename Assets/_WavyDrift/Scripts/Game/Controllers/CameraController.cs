using Racer.Utilities;
using UnityEngine;

/// <summary>
/// Handles camera follow and flipping.
/// </summary>
public class CameraController : MonoBehaviour
{
    float current, target;

    Vector3 offset;

    Vector3 goalRot, initialRot;

    Transform player;

    [Header("Follow")]

    [SerializeField]
    float followSpeed = 5;

    [Header("Flipping")]

    [Space(10), SerializeField]
    float flipSpeed;

    [SerializeField]
    AnimationCurve animationCurve;


    private void Start()
    {
        // Follow
        player = PlayerController.Instance.PlayerMovement.transform;

        offset = transform.position - player.position;

        // Flipping
        initialRot = Utility.CameraMain.transform.rotation.eulerAngles;

        goalRot = new Vector3(initialRot.x, initialRot.y, 180);
    }


    private void FixedUpdate()
    {
        Vector3 desiredPos = player.position + offset;

        // Smoothly follows player.
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.fixedDeltaTime * followSpeed);
    }

    public void FlipRotation()
    {
        target = target == 0 ? 1 : 0;
    }

    private void Update()
    {
        current = Mathf.MoveTowards(current, target, flipSpeed * Time.deltaTime);

        // Smoothly flips the camera to a desired rotation.
        Utility.CameraMain.transform.rotation = Quaternion.Lerp(Quaternion.Euler(initialRot), Quaternion.Euler(goalRot), animationCurve.Evaluate(current));
    }
}
