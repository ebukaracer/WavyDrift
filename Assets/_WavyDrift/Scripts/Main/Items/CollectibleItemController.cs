using Racer.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class CollectibleItemController : MonoBehaviour
{
    private int _collectibleCount;

    private ItemManager _itemManager;
    private CollectibleStore _currentlySelectedItem;

    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerItemController playerItemController;

    [Header("TEXTS"), Space(10)]
    [SerializeField] private TextMeshProUGUI descriptionT;
    [SerializeField] private TextMeshProUGUI priceT;
    [SerializeField] private TextMeshProUGUI currentLevelT;

    [Header("IMAGES"), Space(10)]
    [SerializeField] private Image priceTokenI;

    [Header("BUTTONS"), Space(10)]
    [SerializeField] private Button upgradeB;
    [SerializeField] private Button[] collectibleBtns;


    private void Start()
    {
        _itemManager = ItemManager.Instance;

        _collectibleCount = _itemManager.CollectibleItem.GetItemCount;

        upgradeB.onClick.AddListener(UpgradeItem);

        playerItemController.OnHasPurchasedItem += ItemsController_OnHasPurchasedItem;

        ToggleUnlockItem();
    }

    /// <summary>
    /// Gets an item from 'ItemManager' by its index.
    /// </summary>
    /// <param name="i">Index of the item to retrieve.</param>
    /// <returns>CollectibleItem</returns>
    private CollectibleStore GetCollectible(int i) =>
        _itemManager.CollectibleItem.GetItemByIndex(i);


    /// <summary>
    /// Callback for purchased item notification.
    /// </summary>
    private void ItemsController_OnHasPurchasedItem()
    {
        ToggleUnlockItem();
    }

    /// <summary>
    /// Item currently on focus.
    /// </summary>
    /// <param name="index">Selected Item Index</param>
    public void Select(int index)
    {
        _currentlySelectedItem = GetCollectible(index);

        DisplayCurrentItemInfo();
    }

    /// <summary>
    /// Displays Info about a selected item.
    /// </summary>
    private void DisplayCurrentItemInfo()
    {
        if (_currentlySelectedItem.IsUpgradable)
        {
            ToggleUpgradeButton(true);

            priceT.SetText("{0}", _currentlySelectedItem.LevelPrice);

            currentLevelT.SetText("Level {0}", _currentlySelectedItem.LevelIndex);
        }
        else
        {
            ToggleUpgradeButton(false);

            priceT.text = string.Empty;

            currentLevelT.text = "Max";
        }

        descriptionT.text = _currentlySelectedItem.Description;
    }

    /// <summary>
    /// Upgrades an item, if certain conditions are met.
    /// This would persist till upgrade button is grayed out.
    /// </summary>
    private void UpgradeItem()
    {
        // Unsuccessful
        if (_currentlySelectedItem.LevelPrice > UIControllerMain.Instance.DiamondCount)
        {
            UIControllerMain.Instance.ShowInfoTip("You don't have enough diamonds to upgrade this item!");

            return;
        }

        // Successful
        UIControllerMain.Instance.UpdateDiamonds(_currentlySelectedItem.LevelPrice);


        // Keep upgrading...
        _currentlySelectedItem.LevelPrice += _currentlySelectedItem.PerLevelPrice;

        _currentlySelectedItem.LevelIndex++;

        _currentlySelectedItem.ResourceValue += _currentlySelectedItem.PerResourceValue;

        currentLevelT.SetText("Level {0}", _currentlySelectedItem.LevelIndex);

        priceT.SetText("{0}", _currentlySelectedItem.LevelPrice);

        // Saves upgraded item's data 
        SaveSystem.SaveData($"{_currentlySelectedItem.Title}_Level", _currentlySelectedItem.LevelIndex);

        SaveSystem.SaveData($"{_currentlySelectedItem.Title}_LevelPrice", _currentlySelectedItem.LevelPrice);

        SaveSystem.SaveData($"{_currentlySelectedItem.Title}_ResourceValue", _currentlySelectedItem.ResourceValue);

        // Has reached Max Level
        if (_currentlySelectedItem.LevelIndex >= _currentlySelectedItem.MaxLevelIndex)
        {
            _currentlySelectedItem.IsUpgradable = false;

            SaveSystem.SaveData($"{_currentlySelectedItem.Title}_IsUpgradable", _currentlySelectedItem.IsUpgradable);

            // Disables upgrade button.
            DisplayCurrentItemInfo();
        }
    }


    private void ToggleUpgradeButton(bool state) => upgradeB.interactable = state;

    /// <summary>
    /// Enables a 'CollectibleItem' button, if its matching 'item' has been purchased.
    /// </summary>
    private void ToggleUnlockItem()
    {
        for (int i = 0; i < _collectibleCount; i++)
        {
            collectibleBtns[i].interactable =
                GetCollectible(i).IsUnlocked =
                _itemManager.PlayerItem.GetItemByIndex(i + 2).IsPurchased;
        }
    }

    private void OnDisable()
    {
        playerItemController.OnHasPurchasedItem -= ItemsController_OnHasPurchasedItem;
    }
}