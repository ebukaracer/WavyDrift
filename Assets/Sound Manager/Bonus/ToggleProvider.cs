using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Racer.SoundManager
{
    public enum ToggleState
    {
        Play,
        Stop,
    }

    public abstract class ToggleProvider : MonoBehaviour
    {
        // Placeholder for the current effect.
        // This can be substituted with a bool.
        protected int ToggleIndex;

        public ToggleState toggleState;

        // Handy if you'd save the effect's current state.
        public string saveString;

        [Space(5), Header("Target Graphics")]

        // Parent sprite to be swapped.
        public Image parentIcon;

        // On/Off Sprites that'd be in-place of parent sprite.
        [FormerlySerializedAs("offOnIcons")] public Sprite[] onOffIcons;


        /// <summary>
        /// Retrieves the current state of the Toggle.
        /// </summary>
        /// <remarks>
        /// Should be called on the Start or Awake function.
        /// Invoke your save-class here(retrieval).
        /// </remarks>
        protected abstract void InitToggle();

        /// <summary>
        /// Toggles the current effect On/Off.
        /// </summary>
        /// <remarks>
        /// Should be assigned to a button, in other to achieve a Toggle action.
        /// Invoke your save-class here(saving).
        /// </remarks>
        public virtual void Toggle()
        {
            ToggleIndex++;

            ToggleIndex %= onOffIcons.Length;
        }

        /// <summary>
        /// Syncs the Toggle's current state.
        /// </summary>
        /// <remarks>
        /// Override this method to add extra logic.
        /// </remarks>
        protected virtual void SyncToggle()
        {
            // 0 = play, 1 = stop
            toggleState = ToggleIndex == 0 ? ToggleState.Play : ToggleState.Stop;

            parentIcon.sprite = onOffIcons[ToggleIndex];
        }
    }
}
