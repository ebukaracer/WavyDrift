using UnityEngine;
using UnityEngine.Audio;

namespace Racer.SoundManager
{
    /// <summary>
    /// Manages music and sound effect.
    /// Bound to two audio-sources for music and sound-effects.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField]
        AudioSource sfxSource;

        [SerializeField]
        AudioSource musicSource;

        // When using audio-mixers.
        [Space(10), SerializeField]
        AudioMixerSnapshot[] snapshots;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }


        /// <summary>
        /// Plays a specified audio-clip.
        /// Very applicable for short-looping clips.
        /// This plays over an already playing clip.
        /// </summary>
        /// <param name="clip">Audio-clip to play</param>
        public void PlaySfx(AudioClip clip, float volumeScale = 1)
        {
            if (sfxSource.enabled)
                sfxSource.PlayOneShot(clip, volumeScale);
        }

        /// <summary>
        /// Plays the music clip provided in <see cref="musicSource"/>.
        /// </summary>
        public void PlayMusic()
        {
            if (musicSource.enabled)
                musicSource.Play();
        }

        /// <summary>
        /// Returns a music audio-source reference.
        /// </summary>
        public AudioSource GetMusic() => musicSource;

        /// <summary>
        /// Returns a sound-effect audio-source reference.
        /// </summary>
        public AudioSource GetSfx() => sfxSource;


        // Un/Mute Sound Effect
        public void MuteSfx(bool mute) => sfxSource.mute = mute;

        // Un/Mute Music
        public void MuteMusic(bool mute) => musicSource.mute = mute;

        // Changes the Master Volume.
        // Would affect all active audio-sources.
        public void ChangeMasterVol(float value) => AudioListener.volume = value;


        /// <summary>
        /// Disables/enables a clip.
        /// Alternative to mute but instead disables the audio-source
        /// which prevents it from playing in memory.
        /// </summary>
        public void EnableMusic(bool enabled) => musicSource.enabled = enabled;

        /// <summary>
        /// See also: <seealso cref="EnableMusic(bool)"/>
        /// </summary>
        public void EnableSfx(bool enabled) => sfxSource.enabled = enabled;


        // Smoothly transitions to a snapshot.
        public void MuteAudioMixer(int snapShotIndex, float timeToReach = .1f)
        {
            GetSnapShot(snapShotIndex).TransitionTo(timeToReach);
        }

        public void UnMuteAudioMixer(int snapShotIndex, float timeToReach = .1f)
        {
            GetSnapShot(snapShotIndex).TransitionTo(timeToReach);
        }

        /// <summary>
        /// Returns a snapshot by index.
        /// </summary>
        public AudioMixerSnapshot GetSnapShot(int snapShotIndex)
        {
            return snapshots[snapShotIndex];
        }
    }
}