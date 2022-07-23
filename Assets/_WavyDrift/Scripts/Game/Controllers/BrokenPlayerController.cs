using Racer.Utilities;
using UnityEngine;

/// <summary>
/// Spawns and initializes a particular broken-player-item on start.
/// </summary>
public class BrokenPlayerController : SingletonPattern.Singleton<BrokenPlayerController>
{
    GameObject brokenPlayerToUse;

    public BrokenPlayer BrokenPlayer { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Setup();
    }


    void Setup()
    {
        var itemManager = ItemManager.Instance;

        // Retrieves a broken-player counterpart for the player-item in use
        for (int i = 0; i < itemManager.PlayerItem.GetItemCount; i++)
        {
            var item = itemManager.PlayerItem.GetItemByIndex(i);

            if (item.IsUsing)
            {
                brokenPlayerToUse = item.BrokenPlayerPrefab;

                brokenPlayerToUse.ToggleActive(false);

                break;
            }
        }

        if (brokenPlayerToUse == null)
            return;

        // Assigns the instantiated broken-player to 'BrokenPlayer' reference.
        BrokenPlayer = Instantiate(brokenPlayerToUse, transform).GetComponent<BrokenPlayer>();
    }
}

