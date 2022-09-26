using UnityEngine;

public enum CollectibleType
{
    Coin, Diamond, CoinMagnet, GhostPortion
}

internal class Collectible : MonoBehaviour
{
    public CollectibleType collectibleType;
}

