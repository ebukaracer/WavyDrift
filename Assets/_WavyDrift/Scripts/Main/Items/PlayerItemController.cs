using Racer.SaveSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemController : MonoBehaviour
{
    int pageIndex;

    int itemCount;

    ItemManager itemManager;

    PlayerStore currentlySelectedItem;

    UIControllerMain uiControllerMain;

    public event Action OnHasPurchasedItem;

    [Header("Images")]

    [SerializeField]
    Image currentItemSprite;

    // Coins/Diamonds
    [SerializeField]
    Image unlockToken;

    [SerializeField]
    Sprite unlockPadlock;

    [SerializeField]
    Sprite usingCheckmark;

    [Header("Texts")]
    [Space(10)]

    [SerializeField]
    TextMeshProUGUI currentItemTokenAmtT;

    [SerializeField]
    TextMeshProUGUI currentItemDescriptionT;

    [SerializeField]
    TextMeshProUGUI currentItemNameT;

    [SerializeField]
    TextMeshProUGUI equipT;

    private void Start()
    {
        itemManager = ItemManager.Instance;

        uiControllerMain = UIControllerMain.Instance;


        itemCount = itemManager.PlayerItem.GetItemCount;

        SetRetrievedItemProperties(GetPlayer(pageIndex));

        AvailableItemNotify();
    }

    /// <summary>
    /// Gets a 'PlayerItem' from 'ItemManager' by its index.
    /// </summary>
    /// <param name="i">Index of 'PlayerItem' to retrieve.</param>
    /// <returns>Item</returns>
    PlayerStore GetPlayer(int index) => itemManager.PlayerItem.GetItemByIndex(index);

    /// <summary>
    /// Returns the next item.
    /// Assigns it to current item on focused.
    /// </summary>
    public void NextItem()
    {
        if (pageIndex >= itemCount - 1)
            return;

        pageIndex++;

        SetRetrievedItemProperties(GetPlayer(pageIndex));
    }

    /// <summary>
    /// Returns the previous item.
    /// Assigns it to current item on focused.
    /// </summary>
    public void PreviousItem()
    {
        if (pageIndex <= 0)
            return;

        pageIndex--;

        SetRetrievedItemProperties(GetPlayer(pageIndex));
    }

    /// <summary>
    /// Sets up the properties of the Item in focus.
    /// </summary>
    /// <param name="retrievedItem">The Item in focus.</param>
    private void SetRetrievedItemProperties(PlayerStore retrievedItem)
    {
        currentItemSprite.sprite = retrievedItem.ItemIcon;

        currentItemNameT.text = $"{retrievedItem.Title}";

        currentItemDescriptionT.text = retrievedItem.Description;

        currentlySelectedItem = retrievedItem;


        // Not purchased yet
        if (!retrievedItem.IsPurchased)
        {
            UpdateEquipText("Equip");

            unlockToken.sprite = retrievedItem.UnlockToken;

            currentItemTokenAmtT.SetText("{0}", retrievedItem.TokenAmount);
        }
        // Purchased
        else
        {
            if (retrievedItem.IsUsing)
                unlockToken.sprite = usingCheckmark;
            else
                unlockToken.sprite = unlockPadlock;

            UpdateSomeEquippedProperties();
        }
    }

    /// <summary>
    /// Equips an item that isn't already equipped initially.
    /// </summary>
    public void EquipItem()
    {
        UseItem();

        if (currentlySelectedItem.IsPurchased)
            return;

        // Check item's coins/diamond < player's total coins/diamond.
        switch (currentlySelectedItem.UnlockTokenName)
        {
            case UnlockTokenName.Coins:
                if (currentlySelectedItem.TokenAmount <= uiControllerMain.CoinsCount)
                {
                    // Purchase
                    uiControllerMain.UpdateCoins(currentlySelectedItem.TokenAmount);

                    Equip();
                }
                else
                    uiControllerMain.ShowInfoTip($"You don't have enough Coins to purchase this item!");
                break;

            // Check item's diamond/best-score < player's total diamond/best-score.
            case UnlockTokenName.Diamonds:
                if (currentlySelectedItem.TokenAmount <= uiControllerMain.DiamondCount)
                {
                    if (currentlySelectedItem.DesiredBest <= uiControllerMain.BestCount)
                    {
                        // Purchase
                        uiControllerMain.UpdateDiamonds(currentlySelectedItem.TokenAmount);

                        Equip();
                    }
                    else
                        uiControllerMain.ShowInfoTip($"You must reach a best score of {currentlySelectedItem.DesiredBest} to purchase this item!");
                }
                else
                    uiControllerMain.ShowInfoTip($"You don't have enough Diamonds to purchase this item!");
                break;

            default:
                LogConsole.LogWarning("Could not find any matching unlock token");
                break;
        }
    }

    /// <summary>
    /// Setups equipped item's properties.
    /// </summary>
    private void Equip()
    {
        // Update status
        currentlySelectedItem.IsPurchased = true;

        // Save
        SaveSystem.SaveData($"Unlocked_{currentlySelectedItem.Title}", currentlySelectedItem.IsPurchased);

        // Notify others
        OnHasPurchasedItem?.Invoke();

        // Sets some properties
        unlockToken.sprite = unlockPadlock;

        UpdateSomeEquippedProperties();

        DisplayInfoForEquippedItem();
    }

    /// <summary>
    /// Pop-out that shows details about the equipped item's properties.
    /// </summary>
    void DisplayInfoForEquippedItem()
    {
        switch (currentlySelectedItem.CollectibleNameToUnlock)
        {
            case CollectibleName.Coin_Magnet:
                uiControllerMain.ShowInfoTip("You have successfully unlocked coin magnet, access by navigating to unlocks tab");
                break;
            case CollectibleName.Ghost_Portion:
                uiControllerMain.ShowInfoTip("You have successfully unlocked ghost portion, access by navigating to unlocks tab");
                break;
            case CollectibleName.Coin_Multiplier:
                uiControllerMain.ShowInfoTip("You have successfully unlocked coin multiplier, access by navigating to unlocks tab");
                break;
        }
    }

    /// <summary>
    /// Sets up equipped item's unset properties.
    /// </summary>
    private void UpdateSomeEquippedProperties()
    {
        UpdateEquipText("Use");

        // currentItemDescriptionT.text = "You have unlocked this item!";

        currentItemTokenAmtT.text = string.Empty;
    }

    /// <summary>
    /// Returns; if an item isn't equipped yet.
    /// Disables other items being used and uses the currently selected item.
    /// </summary>
    void UseItem()
    {
        // Checks if item is already equipped/being used.
        if (!currentlySelectedItem.IsPurchased || currentlySelectedItem.IsUsing)
            return;

        // Start using item
        currentlySelectedItem.IsUsing = true;

        unlockToken.sprite = usingCheckmark;

        SaveSystem.SaveData($"Using_{currentlySelectedItem.Title}", currentlySelectedItem.IsUsing);


        // Stop using other items
        for (int i = 0; i < itemCount; i++)
        {
            var thisItem = GetPlayer(i);

            if (thisItem == currentlySelectedItem)
                continue;

            thisItem.IsUsing = false;

            SaveSystem.SaveData($"Using_{thisItem.Title}", thisItem.IsUsing);
        }
    }


    public void UpdateEquipText(string text)
    {
        if (equipT.text != text)
            equipT.text = text;
    }

    /// <summary>
    /// Notification on start, for item/s available for purchase.
    /// </summary>
    void AvailableItemNotify()
    {
        for (int i = 0; i < itemCount; i++)
        {
            var item = GetPlayer(i);

            if (item.IsPurchased || item.IsUsing)
                continue;

            if (item.UnlockTokenName.Equals(UnlockTokenName.Coins) && item.TokenAmount <= uiControllerMain.CoinsCount)
            {
                uiControllerMain.ShowInfoTip($"Item(s) available for purchase!");

                break;
            }

            if ((item.UnlockTokenName.Equals(UnlockTokenName.Diamonds) && item.DesiredBest <= uiControllerMain.BestCount))
            {
                if (item.TokenAmount <= uiControllerMain.DiamondCount)
                {
                    uiControllerMain.ShowInfoTip($"Item(s) available for purchase!");

                    break;
                }
            }
        }
    }
}
