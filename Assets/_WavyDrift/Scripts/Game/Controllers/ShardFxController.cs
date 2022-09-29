using Racer.Utilities;
using UnityEngine;

/// <summary>
/// Controls which shard-type particle effect to spawn.
/// </summary>
internal class ShardFxController : SingletonPattern.Singleton<ShardFxController>
{
    private ParticleSystem _particleSystem;
    private ParticleSystemRenderer _psRenderer;

    [SerializeField, Tooltip("Mesh to assign to the particle system's renderer")]
    private Mesh shield, danger, asteroid, coinMagnet, coin, diamond;


    private void Start()
    {
        _psRenderer = GetComponent<ParticleSystemRenderer>();

        _particleSystem = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Spawns a particle effect(inform of a mesh) that conforms to the actual collectible being used.
    /// </summary>
    /// <param name="collectibleType">Collectible to match with...</param>
    /// <param name="pos">Position to spawn...</param>
    public void InitFx(CollectibleType collectibleType, Vector3 pos)
    {
        switch (collectibleType)
        {
            case CollectibleType.Coin:
                _psRenderer.mesh = coin;
                break;
            case CollectibleType.Diamond:
                _psRenderer.mesh = diamond;
                break;
            case CollectibleType.CoinMagnet:
                _psRenderer.mesh = coinMagnet;
                break;
            case CollectibleType.GhostPortion:
                _psRenderer.mesh = shield;
                break;
        }

        _particleSystem.transform.position = pos;

        _particleSystem.Play();
    }

    /// <summary>
    /// Spawns a particle effect(inform of a mesh) that conforms to the actual damageable being used.
    /// </summary>
    /// <param name="damageableType">Damageable to match with...</param>
    /// <param name="pos">Position to spawn...</param>
    public void InitFx(DamageableType damageableType, Vector3 pos)
    {
        switch (damageableType)
        {
            case DamageableType.Danger:
                _psRenderer.mesh = danger;
                break;
            case DamageableType.Asteroid:
                _psRenderer.mesh = asteroid;
                break;
        }

        _particleSystem.transform.position = pos;

        _particleSystem.Play();
    }
}