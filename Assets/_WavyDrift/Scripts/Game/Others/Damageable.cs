using UnityEngine;

public enum DamageableType
{
    Danger, Asteriod
}

class Damageable : MonoBehaviour
{
    public DamageableType damageableType;
}

