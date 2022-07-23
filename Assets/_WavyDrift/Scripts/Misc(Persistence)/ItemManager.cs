using Racer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Retrieves a scriptable object item.
/// Holds a reference to all the items available in the shop-menu.
/// See also: <seealso cref="ItemInitializer"/>.
/// </summary>
class ItemManager : SingletonPattern.Singleton<ItemManager>
{
    [field: SerializeField]
    public PlayerItem PlayerItem { get; private set; }

    [field: SerializeField, Space(10)]
    public CollectibleItem CollectibleItem { get; private set; }


    private void OnDisable()
    {
        CollectibleItem.ResetProperties();

        PlayerItem.ResetProperties();
    }
}

/// <summary>
/// Manages all collectible-items from the Collectible Store.
/// See also: <seealso cref="CollectibleStore"/>.
/// </summary>
[Serializable]
class CollectibleItem
{
    [SerializeField]
    List<CollectibleStore> collectibles;


    /// <summary>
    /// This returns an item based on how it's assigned and ordered in the list.
    /// </summary>
    /// <param name="index">Index to return</param>
    public CollectibleStore GetItemByIndex(int index)
    {
        if (index >= collectibles.Count || index > collectibles.Count)
        {
            LogConsole.LogWarning($"Invalid Argument: {index}. \nIndex spans from 0 to {collectibles.Count - 1}");

            return null;
        }

        return collectibles[index];
    }

    /// <summary>
    /// This returns an item based on its ID, ordering doesn't necessary matter here.
    /// </summary>
    /// <param name="id">ID of the item to return</param>
    public CollectibleStore GetItemById(int id)
    {
        var item = collectibles.SingleOrDefault(p => p.Id == id);

        if (item == null)
        {
            LogConsole.LogWarning($"Index: {id} does not exist!");

            return null;
        }

        return item;
    }

    public CollectibleStore GetItemByName(CollectibleName name)
    {
        var item = collectibles.SingleOrDefault(n => n.Title == name);

        if (item == null)
        {
            LogConsole.LogWarning($"{nameof(PlayerName)}: {name} does not exist!");

            return null;
        }

        return item;
    }

    /// <summary>
    /// Returns the number of items present in the list.
    /// </summary>
    public int GetItemCount => collectibles.Count;


    public void ResetProperties()
    {
        foreach (var item in collectibles)
            item.ResetState();
    }
}

/// <summary>
/// Manages all player-items from the Player Store.
/// See also: <seealso cref="PlayerStore"/>.
/// </summary>
[Serializable]
class PlayerItem
{
    [SerializeField]
    List<PlayerStore> items;

    /// <summary>
    /// This returns an item based on how it's assigned and ordered in the list.
    /// </summary>
    /// <param name="index">Index to return</param>
    public PlayerStore GetItemByIndex(int index)
    {
        if (index >= items.Count || index > items.Count)
        {
            LogConsole.LogWarning($"Invalid Argument: {index}. \nIndex spans from 0 to {items.Count - 1}");

            return null;
        }

        return items[index];
    }

    /// <summary>
    /// This returns an item based on its ID, ordering doesn't necessarily matter here.
    /// </summary>
    /// <param name="id">ID of the item to return</param>
    public PlayerStore GetItemById(int id)
    {
        var item = items.SingleOrDefault(p => p.Id == id);

        if (item == null)
        {
            LogConsole.LogWarning($"Index: {id} does not exist!");

            return null;
        }

        return item;
    }

    /// <summary>
    /// This returns an item based on its Name.
    /// </summary>
    public PlayerStore GetItemByName(PlayerName name)
    {
        var item = items.SingleOrDefault(n => n.Title == name);

        if (item == null)
        {
            LogConsole.LogWarning($"{nameof(PlayerName)}: {name} does not exist!");

            return null;
        }

        return item;
    }

    /// <summary>
    /// Returns the number of items present in the list.
    /// </summary>
    public int GetItemCount => items.Count;


    /// <summary>
    /// See: <see cref="CollectibleStore.ResetState"/>.
    /// </summary>
    public void ResetProperties()
    {
        foreach (var item in items)
            item.ResetState();
    }
}

