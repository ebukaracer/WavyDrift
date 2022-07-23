using Racer.SoundManager;
using Racer.Utilities;
using System.Collections;
using UnityEngine;


/// <summary>
/// Handles player's motion.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // Private fields
    IEnumerator invisibleDelayCache;

    Vector3 moveForce;

    Vector3 jumpForce;

    Rigidbody ballRb;

    Collider ballSC;

    bool isPressed;

    // Name
    [field: SerializeField]
    public PlayerName ItemName { get; private set; }

    // Motion
    [Space(15)]

    [SerializeField]
    float moveSpeed = default;

    [SerializeField]
    float maxJumpLimit = 50f;

    [SerializeField]
    float bounceForce;

    // Tilting
    [Space(15)]

    [SerializeField, Tooltip("Check this if the player-item is required to tilt")]
    bool shouldTilt;

    [SerializeField]
    float tiltAngle = 45f;

    [SerializeField]
    float tiltSpeed = 8f;

    // Particle Fx
    [Space(10)]

    [SerializeField]
    ParticleSystem respawnFx;

    [SerializeField]
    ParticleSystem dustTrail;

    // SoundFx
    [Space(10)]

    [SerializeField]
    AudioClip trailSfx;

    [SerializeField]
    AudioClip overboardSfx;

    private void Awake()
    {
        ballRb = GetComponent<Rigidbody>();

        ballSC = GetComponent<Collider>();

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
            ballRb.isKinematic = true;
        else
            // UnFreeze
            ballRb.isKinematic = false;

        if (state.Equals(GameStates.GameOver) || state.Equals(GameStates.DestroyWait))
        {
            ballRb.isKinematic = true;

            enabled = false;

            gameObject.ToggleActive(false);
        }
    }

    /// <summary>
    /// Called externally to re-initialize player if set to re-spawn state.
    /// </summary>
    /// <param name="delay">re-spawn delay</param>
    public void Init(float delay)
    {
        ballSC.isTrigger = true;

        ballRb.isKinematic = false;

        enabled = true;

        gameObject.ToggleActive(true);

        ShowRespawnFx(true);

        // Overwrites the existing coroutine instead of waiting for it to finish.
        if (invisibleDelayCache != null)
            StopCoroutine(invisibleDelayCache);

        invisibleDelayCache = InvisibleDelay(delay);

        StartCoroutine(invisibleDelayCache);
    }

    /// <summary>
    /// Simulates a Particle effect to present the player is in its re-spawn state. 
    /// </summary>
    /// <param name="canShow">Plays the particle effect if set to true</param>
    void ShowRespawnFx(bool canShow)
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
            float angle = Mathf.Atan2(-ballRb.velocity.y, tiltAngle) * Mathf.Rad2Deg;

            // Applies rotation the x and z axis
            var tilt = Quaternion.Euler(angle, 0, angle / 2f);

            transform.rotation = Quaternion.Lerp(transform.rotation, tilt, Time.deltaTime * tiltSpeed);
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0))
            isPressed = true;
    }

    private void FixedUpdate()
    {
        moveForce = Vector3.forward * moveSpeed;

        ballRb.AddForce(moveForce * Time.fixedDeltaTime);


        // Applies an immediate upward-force if input is applied
        if (isPressed)
        {
            PlayFx();

            jumpForce = Vector3.up * bounceForce;

            ballRb.AddForce(1000 * Time.fixedDeltaTime * jumpForce);
        }

        // Prevents the player from going overboard.
        if (ballRb.position.y >= maxJumpLimit)
        {
            ballRb.AddForce(1000 * bounceForce * Time.fixedDeltaTime * -Vector3.up);

            SoundManager.Instance.PlaySfx(overboardSfx);

        }

        else if (ballRb.position.y <= -maxJumpLimit)
        {
            ballRb.AddForce(1000 * bounceForce * Time.fixedDeltaTime * Vector3.up);

            SoundManager.Instance.PlaySfx(overboardSfx);
        }


        isPressed = false;
    }

    // Prevents player from colliding with platforms
    IEnumerator InvisibleDelay(float delay)
    {
        yield return Utility.GetWaitForSeconds(delay);

        // After invisibility
        ballSC.isTrigger = false;

        ShowRespawnFx(false);
    }

    /// <summary>
    /// Plays effects(particle/sound) for specific player-items
    /// </summary>
    void PlayFx()
    {
        if (dustTrail == null)
            return;

        dustTrail.Play();

        SoundManager.Instance.PlaySfx(trailSfx);
    }

    private void OnDisable()
    {
        GameManager.OnCurrentState -= GameManager_OnCurrentState;
    }
}