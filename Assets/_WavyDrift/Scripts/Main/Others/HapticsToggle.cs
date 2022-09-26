using Racer.SaveManager;
using Racer.SoundManager;


public class HapticsToggle : ToggleProvider
{
    private void Awake()
    {
        InitToggle();
    }

    protected override void InitToggle()
    {
        ToggleIndex = SaveManager.GetInt(saveString);

        SyncToggle();
    }

    public override void Toggle()
    {
        base.Toggle();

        SaveManager.SaveInt(saveString, ToggleIndex);

        SyncToggle();
    }

    protected override void SyncToggle()
    {
        base.SyncToggle();

        switch (toggleState)
        {
            // default:
            case ToggleState.Play:
                Haptics.Mute(false);
                break;
            case ToggleState.Stop:
                Haptics.Mute(true);
                break;
        }
    }
}

