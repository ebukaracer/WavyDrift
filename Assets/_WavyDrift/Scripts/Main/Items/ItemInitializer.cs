using Racer.SaveSystem;
using UnityEngine;

/// <summary>
/// This initializes all the 'items' available in the shop-menu on startup. 
/// </summary>
internal class ItemInitializer : MonoBehaviour
{
    private ItemManager _itemManager;

    private int _playerItemCount;

    private int _collectibleItemCount;


    private void Awake()
    {
        _itemManager = ItemManager.Instance;

        _playerItemCount = _itemManager.PlayerItem.GetItemCount;

        _collectibleItemCount = _itemManager.CollectibleItem.GetItemCount;


        InitPurchasedPlayerItems();

        InitUsingPlayerItemOnStart();

        InitSavedCollectiblesProperties();
    }


    /// <summary>
    /// Gets an item from 'ItemManager' by its index.
    /// </summary>
    /// <param name="i">Index of the item to retrieve.</param>
    /// <returns>PlayerItem</returns>
    private PlayerStore GetPlayer(int index) =>
        _itemManager.PlayerItem.GetItemByIndex(index);

    /// <summary>
    /// Gets an item from 'ItemManager' by its index.
    /// </summary>
    /// <param name="i">Index of the item to retrieve.</param>
    /// <returns>CollectibleItem</returns>
    private CollectibleStore GetCollectible(int i) =>
        _itemManager.CollectibleItem.GetItemByIndex(i);

    /// <summary>
    /// This retrieves the item that was last selected by the player on startup.
    /// The first item on the shop-menu is being used, if it's player's first time.
    /// </summary>
    private void InitUsingPlayerItemOnStart()
    {
        bool hasSetDefaultItem = false;

        for (int i = 0; i < _playerItemCount; i++)
        {
            if (SaveSystem.GetData<bool>($"Using_{GetPlayer(i).Title}"))
            {
                GetPlayer(i).IsUsing = true;

                hasSetDefaultItem = true;

                break;
            }
        }

        // Default Player Item(item 0; first item on the shop-menu) unlocked
        if (!hasSetDefaultItem)
        {
            // Item[0] set to 'using'.
            GetPlayer(0).IsUsing = true;

            SaveSystem.SaveData($"Using_{GetPlayer(0).Title}", true);
        }
    }

    /// <summary>
    /// Initializes saved purchased 'PlayerItems' on startup.
    /// See also: <seealso cref="InitUsingPlayerItemOnStart"/>
    /// </summary>
    private void InitPurchasedPlayerItems()
    {
        // Default item(item 0) purchased
        GetPlayer(0).IsPurchased = true;

        // Since item[0] is purchased, we loop from item[1].
        for (int i = 1; i < _playerItemCount; i++)
            GetPlayer(i).IsPurchased = SaveSystem.GetData<bool>($"Unlocked_{GetPlayer(i).Title}");
    }

    /// <summary>
    /// Initializes/retrieves all saved-in 'CollectibleItems' values on startup.
    /// </summary>
    private void InitSavedCollectiblesProperties()
    {
        for (int i = 0; i < _collectibleItemCount; i++)
        {
            GetCollectible(i).LevelIndex =
                SaveSystem.GetData($"{GetCollectible(i).Title}_Level", GetCollectible(i).LevelIndex);
            GetCollectible(i).LevelPrice =
                SaveSystem.GetData($"{GetCollectible(i).Title}_LevelPrice", GetCollectible(i).LevelPrice);

            GetCollectible(i).IsUpgradable =
                SaveSystem.GetData($"{GetCollectible(i).Title}_IsUpgradable", GetCollectible(i).IsUpgradable);

            GetCollectible(i).ResourceValue = SaveSystem.GetData($"{GetCollectible(i).Title}_ResourceValue", GetCollectible(i).ResourceValue);

            GetCollectible(i).IsUnlocked = _itemManager.PlayerItem.GetItemByIndex(i + 2).IsPurchased;
        }
    }
}