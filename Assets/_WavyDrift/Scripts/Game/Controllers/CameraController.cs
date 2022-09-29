using Racer.SoundManager;
using Racer.Utilities;
using UnityEngine;

/// <summary>
/// Handles camera follow and flipping.
/// </summary>
internal class CameraController : MonoBehaviour
{
    private float _current, _target;

    private Vector3 _offset, _desiredPos;
    private Vector3 _goalRot, _initialRot;

    private Transform _player;

    [Header("MOTION")]
    [SerializeField] private float followSpeed = 5;
    [SerializeField] private float flipSpeed;

    [Header("OTHERS"), Space(5)]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private AudioClip flipSfx;


    private void Start()
    {
        // Follow
        _player = PlayerController.Instance.PlayerMovement.transform;
        _offset = transform.position - _player.position;

        // Flipping
        _initialRot = Utility.CameraMain.transform.rotation.eulerAngles;
        _goalRot = new Vector3(_initialRot.x, _initialRot.y, 180);
    }


    private void FixedUpdate()
    {
        _desiredPos = _player.position + _offset;

        // Smoothly follows player.
        transform.position = Vector3.Lerp(transform.position, _desiredPos, Time.fixedDeltaTime * followSpeed);
    }

    public void FlipRotation()
    {
        _target = _target == 0 ? 1 : 0;

        SoundManager.Instance.PlaySfx(flipSfx);
    }

    private void Update()
    {
        _current = Mathf.MoveTowards(_current, _target, flipSpeed * Time.deltaTime);

        // Smoothly flips the camera to a desired rotation.
        Utility.CameraMain.transform.rotation = Quaternion.Lerp(Quaternion.Euler(_initialRot), Quaternion.Euler(_goalRot), animationCurve.Evaluate(_current));
    }
}
