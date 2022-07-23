using GDTools.ObjectPooling;
using Racer.SoundManager;
using Racer.Utilities;
using UnityEngine;

/// <summary>
/// Handles player's collision with various items.
/// </summary>
public class PlayerCollider : MonoBehaviour
{
    int multiplier = 1;

    PlayerMovement playerMovement;

    Collectible collectible;

    // A kind of collectible
    Damageable damageable;

    // PoolObject poolObject;

    FillController fillController;

    ShardFxController shardFxController;

    [SerializeField]
    AudioClip collectibleClip;



    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        fillController = FillController.Instance;

        shardFxController = ShardFxController.Instance;

        fillController.GhostFill.OnDecreaseStarted += GhostFill_OnDecreaseStarted;

        var item = ItemManager.Instance.CollectibleItem.GetItemByIndex(0);

        multiplier = item.IsUnlocked ? item.ResourceValue : multiplier;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Falls in trigger with an unlocked collectible.
        if (other.CompareTag("Collectible"))
        {
            SoundManager.Instance.PlaySfx(collectibleClip);

            collectible = other.GetComponent<Collectible>();

            shardFxController.InitFx(collectible.collectibleType, other.transform.position);

            switch (collectible.collectibleType)
            {
                case CollectibleType.Coin:
                    UIControllerGame.Instance.SetCoinT(multiplier);
                    break;
                case CollectibleType.Diamond:
                    UIControllerGame.Instance.SetDiamondT(1);
                    break;
                case CollectibleType.Coin_Magnet:
                    fillController.CoinMagnetFill.DecreaseFill();
                    // Extra Logic
                    break;
                case CollectibleType.Ghost_Portion:
                    fillController.GhostFill.DecreaseFill();
                    // Extra Logic
                    break;
            }

            other.GetComponent<PoolObject>().Despawn(); ;
        }

        // Falls in trigger with an unlocked damageable.
        if (other.CompareTag("Damageable"))
        {
            SoundManager.Instance.PlaySfx(collectibleClip);

            damageable = other.GetComponent<Damageable>();

            shardFxController.InitFx(damageable.damageableType, other.transform.position);

            switch (damageable.damageableType)
            {
                case DamageableType.Danger:
                    fillController.DangerFill.DecreaseTime = Random.Range(5, 8);
                    fillController.DangerFill.DecreaseFill();
                    // Extra Logic
                    break;
                case DamageableType.Asteriod:
                    StartCoroutine(Utility.CameraMain.transform.Shake
                        (Random.Range(.5f, 1f),
                        Random.Range(2f, 4f)));
                    // Extra Logic
                    break;
            }

            other.GetComponent<PoolObject>().Despawn(); ;
        }

        // Falls in trigger with a platform.
        if (other.CompareTag("Boundary"))
        {
            UIControllerGame.Instance.SetScoreT(1);

            return;
        }

        if (other.CompareTag("TriggerScore"))
        {
            UIControllerGame.Instance.SetScoreT(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // collides with a platform.
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Vibrator.Vibrate(500);

            GameManager.Instance.SetGameState(GameStates.DestroyWait);
        }
    }

    private void GhostFill_OnDecreaseStarted()
    {
        playerMovement.Init(fillController.GhostFill.DecreaseTime);
    }
}

