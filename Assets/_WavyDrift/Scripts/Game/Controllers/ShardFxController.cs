using Racer.Utilities;
using UnityEngine;

/// <summary>
/// Controls which shard-type particle effect to spawn.
/// </summary>
public class ShardFxController : SingletonPattern.Singleton<ShardFxController>
{
    ParticleSystemRenderer psRenderer;

    ParticleSystem _particleSystem;

    [SerializeField, Tooltip("Mesh to assign to the particle system's renderer")]
    Mesh coin, diamond, coin_Magnet, shield, danger, asteroid;


    private void Start()
    {
        psRenderer = GetComponent<ParticleSystemRenderer>();

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
                psRenderer.mesh = coin;
                break;
            case CollectibleType.Diamond:
                psRenderer.mesh = diamond;
                break;
            case CollectibleType.Coin_Magnet:
                psRenderer.mesh = coin_Magnet;
                break;
            case CollectibleType.Ghost_Portion:
                psRenderer.mesh = shield;
                break;
        }

        _particleSystem.transform.position = pos;

        _particleSystem.Play();
    }

    /// <summary>
    /// Spawns a particle effect(inform of a mesh) that conforms to the actual damageable being used.
    /// </summary>
    /// <param name="collectibleType">Damageable to match with...</param>
    /// <param name="pos">Position to spawn...</param>
    public void InitFx(DamageableType damageableType, Vector3 pos)
    {
        switch (damageableType)
        {
            case DamageableType.Danger:
                psRenderer.mesh = danger;
                break;
            case DamageableType.Asteriod:
                psRenderer.mesh = asteroid;
                break;
        }

        _particleSystem.transform.position = pos;

        _particleSystem.Play();
    }
}