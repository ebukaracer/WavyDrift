using Racer.SoundManager;
using Racer.Utilities;
using System.Collections;
using UnityEngine;


/// <summary>
/// Handles player's motion.
/// </summary>
internal class PlayerMovement : MonoBehaviour
{
    // Private fields
    private IEnumerator _invisibleDelayCache;

    private Vector3 _moveForce;

    private Vector3 _jumpForce;

    private Rigidbody _ballRb;

    private Collider _ballSc;

    private bool _isPressed;

    private float _angle;

    // Name
    [field: SerializeField]
    public PlayerName ItemName { get; private set; }

    // Motion
    [Space(15)]

    [SerializeField]
    private float moveSpeed;

    [SerializeField] private float maxJumpLimit = 50f;

    [SerializeField] private float bounceForce;

    // Tilting
    [Space(15)]

    [SerializeField, Tooltip("Check this if the player-item is required to tilt")]
    private bool shouldTilt;

    [SerializeField] private float tiltAngle = 45f;

    [SerializeField] private float tiltSpeed = 8f;

    // Particle Fx
    [Space(10)]

    [SerializeField]
    private ParticleSystem respawnFx;

    [SerializeField] private ParticleSystem dustTrail;

    // SoundFx
    [Space(10)]

    [SerializeField]
    private AudioClip trailSfx;

    [SerializeField] private AudioClip overboardSfx;

    private void Awake()
    {
        _ballRb = GetComponent<Rigidbody>();

        _ballSc = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        GameManager.OnCurrentState += GameManager_OnCurrentState;
    }

    /// <summary>
    /// Modifies player-item's settings based on the Game's current state.
    /// See also: <seealso cref="GameManager"/>.
    /// </summary>
    private void GameManager_OnCurrentState(GameStates state)
    {
        if (state.Equals(GameStates.Pause) || state.Equals(GameStates.Loading))
            // Freeze 
            _ballRb.isKinematic = true;
        else
        {
            // UnFreeze
            _isPressed = false;
            _ballRb.isKinematic = false;
        }

        if (!state.Equals(GameStates.GameOver) && !state.Equals(GameStates.DestroyWait)) return;

        _ballRb.isKinematic = true;

        enabled = false;

        gameObject.ToggleActive(false);
    }

    /// <summary>
    /// Called externally to re-initialize player if set to re-spawn state.
    /// </summary>
    /// <param name="delay">re-spawn delay</param>
    public void Init(float delay)
    {
        _ballSc.isTrigger = true;

        _ballRb.isKinematic = false;

        enabled = true;

        gameObject.ToggleActive(true);

        ShowRespawnFx(true);

        // Overwrites the existing coroutine instead of waiting for it to finish.
        if (_invisibleDelayCache != null)
            StopCoroutine(_invisibleDelayCache);

        _invisibleDelayCache = InvisibleDelay(delay);

        StartCoroutine(_invisibleDelayCache);
    }

    /// <summary>
    /// Simulates a Particle effect to present the player is in its re-spawn state. 
    /// </summary>
    /// <param name="canShow">Plays the particle effect if set to true</param>
    private void ShowRespawnFx(bool canShow)
    {
        if (canShow)
            respawnFx.Play();
        else
            respawnFx.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    private void Update()
    {
        // Rotates the player if set to rotate.
        if (shouldTilt)
        {
            _angle = Mathf.Atan2(-_ballRb.velocity.y, tiltAngle) * Mathf.Rad2Deg;

            // Applies rotation the x and z axis
            var tilt = Quaternion.Euler(_angle, 0, _angle / 2f);

            transform.rotation = Quaternion.Lerp(transform.rotation, tilt, Time.deltaTime * tiltSpeed);
        }

        if (Input.GetKeyUp(KeyCode.Space))
            _isPressed = true;
    }

    private void FixedUpdate()
    {
        _moveForce = Vector3.forward * moveSpeed;

        _ballRb.AddForce(_moveForce * Time.fixedDeltaTime);


        // Applies an immediate upward-force if input is applied
        if (_isPressed)
        {
            PlayFx();

            _jumpForce = Vector3.up * bounceForce;

            _ballRb.AddForce(1000 * Time.fixedDeltaTime * _jumpForce);
        }

        // Prevents the player from going overboard.
        if (Mathf.Abs(_ballRb.position.y) > maxJumpLimit)
        {
            _ballRb.AddForce(1000 * bounceForce * Time.fixedDeltaTime * Mathf.Sign(_ballRb.position.y) * -Vector3.up);

            SoundManager.Instance.PlaySfx(overboardSfx, .5f);
        }

        _isPressed = false;
    }

    // Prevents player from colliding with platforms
    private IEnumerator InvisibleDelay(float delay)
    {
        yield return Utility.GetWaitForSeconds(delay);

        // After invisibility
        _ballSc.isTrigger = false;

        ShowRespawnFx(false);
    }

    /// <summary>
    /// Plays effects(particle/sound) for specific player-items
    /// </summary>
    private void PlayFx()
    {
        if (dustTrail == null)
            return;

        dustTrail.Play();

        SoundManager.Instance.PlaySfx(trailSfx, .5f);
    }

    private void OnDisable()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}