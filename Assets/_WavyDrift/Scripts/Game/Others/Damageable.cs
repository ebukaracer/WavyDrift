using UnityEngine;

public enum DamageableType
{
    Danger, Asteroid
}

internal class Damageable : MonoBehaviour
{
    public DamageableType damageableType;
}

