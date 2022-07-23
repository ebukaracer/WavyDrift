using Racer.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleItemController : MonoBehaviour
{
    int collectibleCount;

    ItemManager itemManager;

    CollectibleStore currentlySelectedItem;

    // Script References
    [Header("Script ref")]

    [SerializeField]
    PlayerItemController playerItemController;

    // Texts
    [Header("Texts"), Space(10)]

    [SerializeField]
    TextMeshProUGUI descriptionT;

    [SerializeField]
    TextMeshProUGUI priceT;

    [SerializeField]
    TextMeshProUGUI currentLevelT;

    // Images
    [Header("Images"), Space(10)]

    [SerializeField]
    Image priceTokenI;

    // Buttons
    [Header("Buttons"), Space(10)]

    [SerializeField]
    Button upgradeB;

    [SerializeField]
    Button[] collectibleBtns;


    private void Start()
    {
        itemManager = ItemManager.Instance;

        collectibleCount = itemManager.CollectibleItem.GetItemCount;


        upgradeB.onClick.AddListener(UpgradeItem);

        playerItemController.OnHasPurchasedItem += ItemsController_OnHasPurchasedItem;

        ToggleUnlockItem();
    }

    /// <summary>
    /// Gets an item from 'ItemManager' by its index.
    /// </summary>
    /// <param name="i">Index of the item to retrieve.</param>
    /// <returns>CollectibleItem</returns>
    CollectibleStore GetCollectible(int i) =>
        itemManager.CollectibleItem.GetItemByIndex(i);



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
        currentlySelectedItem = GetCollectible(index);

        DisplayCurrentItemInfo();
    }

    /// <summary>
    /// Displays Info about a selected item.
    /// </summary>
    void DisplayCurrentItemInfo()
    {
        if (currentlySelectedItem.IsUpgradable)
        {
            ToggleUpgradeButton(true);

            priceT.SetText("{0}", currentlySelectedItem.LevelPrice);

            currentLevelT.SetText("Level {0}", currentlySelectedItem.LevelIndex);
        }
        else
        {
            ToggleUpgradeButton(false);

            priceT.text = string.Empty;

            currentLevelT.text = "Max";
        }

        descriptionT.text = currentlySelectedItem.Description;
    }

    /// <summary>
    /// Upgrades an item, if certain conditions are met.
    /// This would persist till upgrade button is grayed out.
    /// </summary>
    void UpgradeItem()
    {
        // Unsuccessful
        if (currentlySelectedItem.LevelPrice > UIControllerMain.Instance.DiamondCount)
        {
            UIControllerMain.Instance.ShowInfoTip("You don't have enough diamonds to upgrade this item!");

            return;
        }

        // Successful
        UIControllerMain.Instance.UpdateDiamonds(currentlySelectedItem.LevelPrice);


        // Keep upgrading...
        currentlySelectedItem.LevelPrice += currentlySelectedItem.PerLevelPrice;

        currentlySelectedItem.LevelIndex++;

        currentlySelectedItem.ResourceValue += currentlySelectedItem.PerResourceValue;

        currentLevelT.SetText("Level {0}", currentlySelectedItem.LevelIndex);

        priceT.SetText("{0}", currentlySelectedItem.LevelPrice);

        // Saves upgraded item's data 
        SaveSystem.SaveData($"{currentlySelectedItem.Title}_Level", currentlySelectedItem.LevelIndex);

        SaveSystem.SaveData($"{currentlySelectedItem.Title}_LevelPrice", currentlySelectedItem.LevelPrice);

        SaveSystem.SaveData($"{currentlySelectedItem.Title}_ResourceValue", currentlySelectedItem.ResourceValue);

        // Has reached Max Level
        if (currentlySelectedItem.LevelIndex >= currentlySelectedItem.MaxLevelIndex)
        {
            currentlySelectedItem.IsUpgradable = false;

            SaveSystem.SaveData($"{currentlySelectedItem.Title}_IsUpgradable", currentlySelectedItem.IsUpgradable);

            // Disables upgrade button.
            DisplayCurrentItemInfo();
        }
    }


    void ToggleUpgradeButton(bool state) => upgradeB.interactable = state;

    /// <summary>
    /// Enables a 'CollectibleItem' button, if its matching 'item' has been purchased.
    /// </summary>
    void ToggleUnlockItem()
    {
        for (int i = 0; i < collectibleCount; i++)
        {
            collectibleBtns[i].interactable =
                GetCollectible(i).IsUnlocked = 
                itemManager.PlayerItem.GetItemByIndex(i + 2).IsPurchased;
        }
    }
}