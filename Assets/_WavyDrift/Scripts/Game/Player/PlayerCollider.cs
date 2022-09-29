using Racer.ObjectPooler;
using Racer.SoundManager;
using Racer.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles player's collision with various items.
/// </summary>
internal class PlayerCollider : MonoBehaviour
{
    private int _multiplier = 1;

    private PlayerMovement _playerMovement;
    private Collectible _collectible;
    private Damageable _damageable;

    private FillController _fillController;
    private ShardFxController _shardFxController;

    [SerializeField] private AudioClip collectibleClip;


    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        _fillController = FillController.Instance;
        _shardFxController = ShardFxController.Instance;

        _fillController.GhostFill.OnDecreaseStarted += GhostFill_OnDecreaseStarted;

        var item = ItemManager.Instance.CollectibleItem.GetItemByIndex(0);

        _multiplier = item.IsUnlocked ? item.ResourceValue : _multiplier;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Falls in trigger with an unlocked collectible.
        if (other.CompareTag("Collectible"))
        {
            SoundManager.Instance.PlaySfx(collectibleClip);

            _collectible = other.GetComponent<Collectible>();

            _shardFxController.InitFx(_collectible.collectibleType, other.transform.position);

            switch (_collectible.collectibleType)
            {
                case CollectibleType.Coin:
                    UIControllerGame.Instance.SetCoinT(_multiplier);
                    break;
                case CollectibleType.Diamond:
                    UIControllerGame.Instance.SetDiamondT(1);
                    break;
                case CollectibleType.CoinMagnet:
                    _fillController.CoinMagnetFill.DecreaseFill();
                    // Extra Logic
                    break;
                case CollectibleType.GhostPortion:
                    _fillController.GhostFill.DecreaseFill();
                    // Extra Logic
                    break;
            }

            other.GetComponent<PoolObject>().Despawn();
        }

        // Falls in trigger with an unlocked damageable.
        if (other.CompareTag("Damageable"))
        {
            SoundManager.Instance.PlaySfx(collectibleClip);

            _damageable = other.GetComponent<Damageable>();

            _shardFxController.InitFx(_damageable.damageableType, other.transform.position);

            switch (_damageable.damageableType)
            {
                case DamageableType.Danger:
                    if (_fillController.DangerFill.IsRoutineBusy) return;
                    _fillController.DangerFill.DecreaseTime = Random.Range(5, 9);
                    _fillController.DangerFill.DecreaseFill();
                    // Extra Logic
                    break;
                case DamageableType.Asteroid:
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
            Haptics.Vibrate(500);

            GameManager.Instance.SetGameState(GameStates.DestroyWait);
        }
    }

    private void GhostFill_OnDecreaseStarted()
    {
        _playerMovement.Init(_fillController.GhostFill.DecreaseTime);
    }

    private void OnDestroy()
    {
        _fillController.GhostFill.OnDecreaseStarted -= GhostFill_OnDecreaseStarted;
    }
}

