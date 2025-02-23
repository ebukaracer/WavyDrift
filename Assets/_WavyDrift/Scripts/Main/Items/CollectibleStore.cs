﻿using UnityEngine;

internal enum CollectibleName
{
    Null, CoinMagnet, GhostPortion, CoinMultiplier
}

/// <summary>
/// A store that accumulates all un-lockable collectibles.
/// <see cref="CollectibleName.CoinMultiplier"/> is an imaginary collectible.
/// </summary>
[CreateAssetMenu(fileName = "Collectible_0", menuName = "Collectible")]
internal class CollectibleStore : ScriptableObject
{
    // Details
    [SerializeField] private int id;

    [SerializeField] private CollectibleName title;

    [SerializeField, Multiline] private string description;

    // Price
    [field: SerializeField, Space(10)]
    public int LevelPrice { get; set; }

    [SerializeField, Tooltip("Accepts same value as 'LevelPrice'.")]
    private int levelPriceCache;

    [SerializeField] private int perLevelPrice;

    // Properties
    /// <summary>
    /// The item's current level as it gets upgraded.
    /// </summary>
    [field: SerializeField, Space(10)]
    public int LevelIndex { get; set; }

    [SerializeField] private int maxLevelIndex;

    [SerializeField, Space(10)] private int perResourceValue;

    /// <summary>
    /// A value that it'd add to the item when unlocked and being used.
    /// The value in this case is 'delay' increment.
    /// </summary>
    [field: SerializeField]
    public int ResourceValue { get; set; }

    [SerializeField, Tooltip("Accepts same value as resource value.")]
    private int resourceValueCache;

    [field: SerializeField, Space(10)]
    public bool IsUnlocked { get; set; }

    [field: SerializeField]
    public bool IsUpgradable { get; set; }

    [SerializeField, Tooltip("Accepts same value as IsUpgradable.")]
    private bool isUpgradableCache;

    // Getters
    public int Id => id;

    public int MaxLevelIndex => maxLevelIndex;

    public int PerLevelPrice => perLevelPrice;

    public int PerResourceValue => perResourceValue;

    public CollectibleName Title => title;

    public string Description => description;

    /// <summary>
    /// This is called to reset the values being changed at runtime,
    /// since SO's manipulated data doesn't get reset after play mode. 
    /// </summary>
    public void ResetState()
    {
        LevelPrice = levelPriceCache;

        ResourceValue = resourceValueCache;

        IsUpgradable = isUpgradableCache;

        LevelIndex = 1;

        IsUnlocked = false;
    }
}

