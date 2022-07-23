using Racer.SoundManager;
using Racer.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables a broken-player when required.
/// </summary>
public class BrokenPlayer : MonoBehaviour
{
    bool hasAccessed;

    FillBar radialFill;

    [SerializeField]
    List<Rigidbody> rb;

    [SerializeField, Space(5)]
    ParticleSystem ps;

    [SerializeField, Space(5)]
    float destroyDelay = 2.5f;

    [SerializeField]
    float explosionForce = 2500f;

    [SerializeField, Space(5)]
    AudioClip decimateFx;

    // For the rigid bodies: set 'IsKinamatics' true,
    // Set drag to 0.

    private void OnEnable()
    {
        // Prevents re-initializing 'radialFill' every time 'OnEnable' is called.
        if (!hasAccessed)
        {
            radialFill = FillController.Instance.RadialFill;

            hasAccessed = true;
        }

        // Subscription
        radialFill.OnDecreaseFinished += DelayRadial_OnDecreaseFinished;

        StartCoroutine(DecimateSetupDelay());
    }

    /// <summary>
    /// Callback that triggers whenever the 'radialFill' time has elapsed.
    /// </summary>
    private void DelayRadial_OnDecreaseFinished()
    {
        GameManager.Instance.SetGameState(GameStates.GameOver);

        SoundManager.Instance.PlaySfx(decimateFx);

        ps.Stop();

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
        radialFill.DecreaseTime = destroyDelay;

        // Triggers the radial-fill that displays the time left.
        radialFill.DecreaseFill();
    }

    /// <summary>
    /// A little delay before disabling broken-player pieces.
    /// </summary>
    IEnumerator DisablePiecesDelay()
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
        radialFill.IsStopRoutine = state;
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
        radialFill.OnDecreaseFinished -= DelayRadial_OnDecreaseFinished;

        StopCoroutine(DecimateSetupDelay());
    }
}
