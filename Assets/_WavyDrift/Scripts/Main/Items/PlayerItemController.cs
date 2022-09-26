using Racer.SaveSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class PlayerItemController : MonoBehaviour
{
    private int _pageIndex;

    private int _itemCount;

    private ItemManager _itemManager;

    private PlayerStore _currentlySelectedItem;

    private UIControllerMain _uiControllerMain;

    public event Action OnHasPurchasedItem;

    [Header("Images")]

    [SerializeField]
    private Image currentItemSprite;

    // Coins/Diamonds
    [SerializeField] private Image unlockToken;

    [SerializeField] private Sprite unlockPadlock;

    [SerializeField] private Sprite usingCheckmark;

    [Header("Texts"), Space(10)]

    [SerializeField] private TextMeshProUGUI currentItemTokenAmtT;

    [SerializeField] private TextMeshProUGUI currentItemDescriptionT;

    [SerializeField] private TextMeshProUGUI currentItemNameT;

    [SerializeField] private TextMeshProUGUI equipT;

    private void Start()
    {
        _itemManager = ItemManager.Instance;

        _uiControllerMain = UIControllerMain.Instance;


        _itemCount = _itemManager.PlayerItem.GetItemCount;

        SetRetrievedItemProperties(GetPlayer(_pageIndex));

        AvailableItemNotify();
    }

    /// <summary>
    /// Gets a 'PlayerItem' from 'ItemManager' by its index.
    /// </summary>
    /// <param name="index">Index of 'PlayerItem' to retrieve.</param>
    /// <returns>Item</returns>
    private PlayerStore GetPlayer(int index) => _itemManager.PlayerItem.GetItemByIndex(index);

    /// <summary>
    /// Returns the next item.
    /// Assigns it to current item on focused.
    /// </summary>
    public void NextItem()
    {
        if (_pageIndex >= _itemCount - 1)
            return;

        _pageIndex++;

        SetRetrievedItemProperties(GetPlayer(_pageIndex));
    }

    /// <summary>
    /// Returns the previous item.
    /// Assigns it to current item on focused.
    /// </summary>
    public void PreviousItem()
    {
        if (_pageIndex <= 0)
            return;

        _pageIndex--;

        SetRetrievedItemProperties(GetPlayer(_pageIndex));
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

        _currentlySelectedItem = retrievedItem;


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

        if (_currentlySelectedItem.IsPurchased)
            return;

        // Check item's coins/diamond < player's total coins/diamond.
        switch (_currentlySelectedItem.UnlockTokenName)
        {
            case UnlockTokenName.Coins:
                if (_currentlySelectedItem.TokenAmount <= _uiControllerMain.CoinsCount)
                {
                    // Purchase
                    _uiControllerMain.UpdateCoins(_currentlySelectedItem.TokenAmount);

                    Equip();
                }
                else
                    _uiControllerMain.ShowInfoTip($"You don't have enough Coins to purchase this item!");
                break;

            // Check item's diamond/best-score < player's total diamond/best-score.
            case UnlockTokenName.Diamonds:
                if (_currentlySelectedItem.TokenAmount <= _uiControllerMain.DiamondCount)
                {
                    if (_currentlySelectedItem.DesiredBest <= _uiControllerMain.BestCount)
                    {
                        // Purchase
                        _uiControllerMain.UpdateDiamonds(_currentlySelectedItem.TokenAmount);

                        Equip();
                    }
                    else
                        _uiControllerMain.ShowInfoTip($"You must reach a best score of {_currentlySelectedItem.DesiredBest} to purchase this item!");
                }
                else
                    _uiControllerMain.ShowInfoTip($"You don't have enough Diamonds to purchase this item!");
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
        _currentlySelectedItem.IsPurchased = true;

        // Save
        SaveSystem.SaveData($"Unlocked_{_currentlySelectedItem.Title}", _currentlySelectedItem.IsPurchased);

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
    private void DisplayInfoForEquippedItem()
    {
        switch (_currentlySelectedItem.CollectibleNameToUnlock)
        {
            case CollectibleName.CoinMagnet:
                _uiControllerMain.ShowInfoTip("You have successfully unlocked coin magnet, access by navigating to unlocks tab");
                break;
            case CollectibleName.GhostPortion:
                _uiControllerMain.ShowInfoTip("You have successfully unlocked ghost portion, access by navigating to unlocks tab");
                break;
            case CollectibleName.CoinMultiplier:
                _uiControllerMain.ShowInfoTip("You have successfully unlocked coin multiplier, access by navigating to unlocks tab");
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
    private void UseItem()
    {
        // Checks if item is already equipped/being used.
        if (!_currentlySelectedItem.IsPurchased || _currentlySelectedItem.IsUsing)
            return;

        // Start using item
        _currentlySelectedItem.IsUsing = true;

        unlockToken.sprite = usingCheckmark;

        SaveSystem.SaveData($"Using_{_currentlySelectedItem.Title}", _currentlySelectedItem.IsUsing);


        // Stop using other items
        for (int i = 0; i < _itemCount; i++)
        {
            var thisItem = GetPlayer(i);

            if (thisItem == _currentlySelectedItem)
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
    private void AvailableItemNotify()
    {
        for (int i = 0; i < _itemCount; i++)
        {
            var item = GetPlayer(i);

            if (item.IsPurchased || item.IsUsing)
                continue;

            if (item.UnlockTokenName.Equals(UnlockTokenName.Coins) && item.TokenAmount <= _uiControllerMain.CoinsCount)
            {
                _uiControllerMain.ShowInfoTip($"Item(s) available for purchase!");

                break;
            }

            if ((item.UnlockTokenName.Equals(UnlockTokenName.Diamonds) && item.DesiredBest <= _uiControllerMain.BestCount))
            {
                if (item.TokenAmount <= _uiControllerMain.DiamondCount)
                {
                    _uiControllerMain.ShowInfoTip($"Item(s) available for purchase!");

                    break;
                }
            }
        }
    }
}
