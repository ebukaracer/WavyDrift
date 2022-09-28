using Racer.SaveManager;
using Racer.SoundManager;
using UnityEngine;


// Post-Processing Toggle, changes visible in the Game Scene.
public class PostCcToggle : ToggleProvider
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

        switch (toggleState)
        {
            case ToggleState.Play:
                UIControllerMain.Instance.ShowInfoTip($"Post Processing Enabled, see changes in the Game Scene.");
                break;
            case ToggleState.Stop:
                UIControllerMain.Instance.ShowInfoTip($"Post Processing Disabled, see changes in the Game Scene.");
                break;
        }
    }
}

