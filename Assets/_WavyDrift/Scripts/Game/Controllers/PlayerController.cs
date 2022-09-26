using UnityEngine;
using Racer.Utilities;

/// <summary>
/// Spawns and initializes a particular player-item on start.
/// </summary>
internal class PlayerController : SingletonPattern.StaticInstance<PlayerController>
{
    private GameObject _playerToUse;

    public PlayerMovement PlayerMovement { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Setup();
    }


    private void Setup()
    {
        var itemManager = ItemManager.Instance;

        // Retrieves an unlocked player from 'PlayerShop'
        for (int i = 0; i < itemManager.PlayerItem.GetItemCount; i++)
        {
            var item = itemManager.PlayerItem.GetItemByIndex(i);

            if (!item.IsUsing) continue;

            _playerToUse = item.PlayerPrefab;

            break;
        }

        if (_playerToUse == null)
            return;

        // Assigns the instantiated player to 'PlayerMovement' reference.
        PlayerMovement = Instantiate(_playerToUse, transform).GetComponent<PlayerMovement>();
    }
}