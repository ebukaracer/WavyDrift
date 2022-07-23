using UnityEngine;

public enum CollectibleType
{
    Coin, Diamond, Coin_Magnet, Ghost_Portion
}

class Collectible : MonoBehaviour
{
    public CollectibleType collectibleType;
}

