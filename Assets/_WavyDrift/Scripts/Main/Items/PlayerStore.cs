using UnityEngine;


public enum PlayerName { Wavyball, Kite, Umbrella, Jet, Jetpackboy }

public enum UnlockTokenName { Coins, Diamonds }


/// <summary>
/// This stores all player-items to be purchased.
/// </summary>
[CreateAssetMenu(fileName = "Player_0", menuName = "PlayerItem", order = 2)]
public class PlayerStore : ScriptableObject
{
    [SerializeField]
    int id;

    [SerializeField]
    PlayerName title;

    [SerializeField, Multiline]
    string description;


    [SerializeField, Space(10)]
    Sprite icon;


    [Space(10), SerializeField]
    Sprite unlockTokenIcon;

    [field: SerializeField]
    public UnlockTokenName UnlockTokenName { get; private set; }

    [SerializeField]
    int tokenAmount;


    [field: SerializeField, Space(10)]
    public int DesiredBest { get; private set; }

    /// <summary>
    /// The collectible to unlock when this item is purchased.
    /// See also: <seealso cref="CollectibleStore"/>.
    /// </summary>
    [field: SerializeField, Space(10)]
    public CollectibleName CollectibleNameToUnlock { get; private set; }


    [field: SerializeField, Space(10)]
    public bool IsPurchased { get; set; }

    [field: SerializeField]
    public bool IsUsing { get; set; }

    /// <summary>
    /// Player to spawn when game begins see also: <seealso cref="PlayerController"/>.
    /// </summary>
    [field: SerializeField, Space(10)]
    public GameObject PlayerPrefab { get; private set; }

    /// <summary>
    /// Broken player to spawn when game begins, see also: <seealso cref="BrokenPlayerController"/>.
    /// </summary>
    [field: SerializeField]
    public GameObject BrokenPlayerPrefab { get; private set; }


    // Getters
    public int Id => id;

    public PlayerName Title => title;

    public string Description => description;

    public Sprite ItemIcon => icon;

    public Sprite UnlockToken => unlockTokenIcon;

    public int TokenAmount => tokenAmount;

    /// <summary>
    /// This is called to reset the values being changed at runtime,
    /// since SO's manipulated data doesn't get reset after play mode. 
    /// </summary>
    public void ResetState()
    {
        IsPurchased = false;

        IsUsing = false;
    }
}

