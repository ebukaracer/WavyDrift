using Racer.SaveManager;
using Racer.SoundManager;


public class NoNameToggle : ToggleProvider
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

        SyncToggle();
    }
}

