using UnityEngine;
using Racer.Utilities;

/// <summary>
/// Spawns and initializes a particular player-item on start.
/// </summary>
public class PlayerController : SingletonPattern.StaticInstance<PlayerController>
{
    GameObject playerToUse;

    public PlayerMovement PlayerMovement { get; private set; } = null;

    protected override void Awake()
    {
        base.Awake();

        Setup();
    }


    void Setup()
    {
        var itemManager = ItemManager.Instance;

        // Retrieves an unlocked player from 'PlayerShop'
        for (int i = 0; i < itemManager.PlayerItem.GetItemCount; i++)
        {
            var item = itemManager.PlayerItem.GetItemByIndex(i);

            if (item.IsUsing)
            {
                playerToUse = item.PlayerPrefab;

                break;
            }
        }

        if (playerToUse == null)
            return;

        // Assigns the instantiated player to 'PlayerMovement' reference.
        PlayerMovement = Instantiate(playerToUse, transform).GetComponent<PlayerMovement>();
    }
}