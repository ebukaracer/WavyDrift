using Racer.SaveManager;
using Racer.SoundManager;

internal class SoundToggle : ToggleProvider
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
                SoundManager.Instance.EnableMusic(true);
                SoundManager.Instance.EnableSfx(true);
                break;
            case ToggleState.Stop:
                SoundManager.Instance.EnableMusic(false);
                SoundManager.Instance.EnableSfx(false);
                break;
        }
    }
}
