using UnityEngine;

namespace Racer.SoundManager
{
    /// <summary>
    /// Plays a specific sound-effect.
    /// </summary>
    class ButtonSfx : MonoBehaviour
    {
        [SerializeField]
        AudioClip clip;


        public void Play()
        {
            // Usage
            SoundManager.Instance.PlaySfx(clip);
        }
    }
}