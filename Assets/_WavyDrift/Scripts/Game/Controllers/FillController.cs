using System;
using Racer.Utilities;
using UnityEngine;

/// <summary>
/// Retrieves a <see cref="FillBar"/> reference.
/// See also: <seealso cref="FillBar"/>
/// </summary>
internal class FillController : SingletonPattern.Singleton<FillController>
{
    [SerializeField] private CameraController cameraController;

    // UI Fill-Bars available
    [field: SerializeField, Space(10)]
    public FillBar CoinMagnetFill { get; private set; }

    [field: SerializeField]
    public FillBar DangerFill { get; private set; }

    [field: SerializeField]
    public FillBar GhostFill { get; private set; }

    [field: SerializeField]
    public FillBar RadialFill { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        var item = ItemManager.Instance.CollectibleItem;

        CoinMagnetFill.DecreaseTime = item.GetItemByName(CollectibleName.CoinMagnet).ResourceValue;

        GhostFill.DecreaseTime = item.GetItemByName(CollectibleName.GhostPortion).ResourceValue;
    }

    private void Start()
    {
        // Danger_Fill Subscription
        DangerFill.OnDecreaseStarted += DangerDecreaseStarted;

        DangerFill.OnDecreaseFinished += DangerDecreaseFinished;
    }

    private void DangerDecreaseStarted()
    {
        cameraController.FlipRotation();
    }

    private void DangerDecreaseFinished()
    {
        cameraController.FlipRotation();
    }

    private void OnDisable()
    {
        DangerFill.OnDecreaseStarted -= DangerDecreaseStarted;

        DangerFill.OnDecreaseFinished -= DangerDecreaseFinished;
    }
}