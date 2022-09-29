using Racer.SoundManager;
using Racer.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Enables a broken-player when required.
/// </summary>
internal class BrokenPlayer : MonoBehaviour
{
    private bool _hasAccessed;

    private FillBar _radialFill;

    [Header("BROKEN PLAYER RIGIDBODIES"), Space(5)]
    [SerializeField] private List<Rigidbody> rb;

    [Header("PARTICLE FX"), Space(5)]
    [FormerlySerializedAs("ps")][SerializeField] private ParticleSystem smokeFx;

    [SerializeField] private float destroyDelay = 2.5f;
    [SerializeField] private float explosionForce = 2500f;

    [SerializeField, Space(5)] private AudioClip decimateFx;

    // For the rigid bodies: set 'IsKinematics' true,
    // Set drag to 0.

    private void OnEnable()
    {
        // Prevents re-initializing 'radialFill' every time 'OnEnable' is called.
        if (!_hasAccessed)
        {
            _radialFill = FillController.Instance.RadialFill;

            _hasAccessed = true;
        }

        // Subscription
        _radialFill.OnDecreaseFinished += DelayRadial_OnDecreaseFinished;

        StartCoroutine(DecimateSetupDelay());
    }

    /// <summary>
    /// Callback that triggers whenever the 'radialFill' time has elapsed.
    /// </summary>
    private void DelayRadial_OnDecreaseFinished()
    {
        GameManager.Instance.SetGameState(GameStates.GameOver);

        SoundManager.Instance.PlaySfx(decimateFx);

        smokeFx.Stop();

        foreach (var rbs in rb)
        {
            rbs.isKinematic = false;

            rbs.AddExplosionForce(explosionForce, rbs.transform.parent.position, 7f, .5f);
        }

        StartCoroutine(DisablePiecesDelay());
    }

    /// <summary>
    /// Initializes 'radialFill' timing when it is displayed for the current session.
    /// </summary>
    private IEnumerator DecimateSetupDelay()
    {
        // Make sure a count-down is not running before starting a new one.
        StopCountdown(false);

        // UI animator time to setup.
        yield return Utility.GetWaitForSeconds(.35f);

        // Time to elapse.
        _radialFill.DecreaseTime = destroyDelay;

        // Triggers the radial-fill that displays the time left.
        _radialFill.DecreaseFill();
    }

    /// <summary>
    /// A little delay before disabling broken-player pieces.
    /// </summary>
    private IEnumerator DisablePiecesDelay()
    {
        yield return Utility.GetWaitForSeconds(destroyDelay + 3.5f);

        foreach (var rbs in rb)
        {
            rbs.freezeRotation = true;

            if (rbs.position.y < -100f)
                rbs.gameObject.ToggleActive(false);
        }
    }

    /// <summary>
    /// See: <see cref="FillBar"/>.
    /// </summary>
    /// <param name="state"></param>
    public void StopCountdown(bool state)
    {
        _radialFill.IsStopRoutine = state;
    }

    /// <summary>
    /// Enables a broken-player of the player's counterpart.
    /// </summary>
    public void InitializeOnStartup(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);

        gameObject.ToggleActive(true);
    }

    /// <summary>
    /// Stops all timers and callbacks.
    /// </summary>
    private void OnDisable()
    {
        StopCountdown(true);

        // Unregister from callback
        _radialFill.OnDecreaseFinished -= DelayRadial_OnDecreaseFinished;

        StopCoroutine(nameof(DecimateSetupDelay));
    }
}
