using Racer.Utilities;
using UnityEngine;

/// <summary>
/// Spawns and initializes a particular broken-player-item on start.
/// </summary>
internal class BrokenPlayerController : SingletonPattern.Singleton<BrokenPlayerController>
{
    private GameObject _brokenPlayerToUse;

    public BrokenPlayer BrokenPlayer { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Setup();
    }


    private void Setup()
    {
        var itemManager = ItemManager.Instance;

        // Retrieves a broken-player counterpart for the player-item in use
        for (int i = 0; i < itemManager.PlayerItem.GetItemCount; i++)
        {
            var item = itemManager.PlayerItem.GetItemByIndex(i);

            if (!item.IsUsing) continue;

            _brokenPlayerToUse = item.BrokenPlayerPrefab;

            _brokenPlayerToUse.ToggleActive(false);

            break;
        }

        if (_brokenPlayerToUse == null)
            return;

        // Assigns the instantiated broken-player to 'BrokenPlayer' reference.
        BrokenPlayer = Instantiate(_brokenPlayerToUse, transform).GetComponent<BrokenPlayer>();
    }
}

