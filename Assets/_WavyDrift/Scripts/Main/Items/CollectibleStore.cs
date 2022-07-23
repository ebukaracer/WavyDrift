using UnityEngine;

public enum CollectibleName
{
    Null, Coin_Magnet, Ghost_Portion, Coin_Multiplier
}

/// <summary>
/// A store that accumulates all un-lockable collectibles.
/// <see cref="CollectibleName.Coin_Multiplier"/> is an imaginary collectible.
/// </summary>
[CreateAssetMenu(fileName = "Collectible_0", menuName = "Collectible")]
public class CollectibleStore : ScriptableObject
{
    [SerializeField]
    int id;

    [SerializeField]
    CollectibleName title;

    [SerializeField, Multiline]
    string description;

    [field: SerializeField, Space(10)]
    public int LevelPrice { get; set; }

    [SerializeField, Tooltip("Accepts same value as 'LevelPrice'.")]
    int levelPriceCache;

    [SerializeField]
    int perLevelPrice;

    /// <summary>
    /// The item's current level as it gets upgraded.
    /// </summary>
    [field: SerializeField, Space(10)]
    public int LevelIndex { get; set; }

    [SerializeField]
    int maxLevelIndex;


    [SerializeField, Space(10)]
    int perResourceValue;

    /// <summary>
    /// A value that it'd add to the item when unlocked and being used.
    /// The value in this case is 'delay' increment.
    /// </summary>
    [field: SerializeField]
    public int ResourceValue { get; set; }

    [SerializeField, Tooltip("Accepts same value as resource value.")]
    int resourceValueCache;


    [field: SerializeField, Space(10)]
    public bool IsUnlocked { get; set; }

    [field: SerializeField]
    public bool IsUpgradable { get; set; }

    [SerializeField, Tooltip("Accepts same value as IsUpgradable.")]
    bool isUpgradableCache;

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

