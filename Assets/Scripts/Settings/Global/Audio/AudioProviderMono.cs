using System;
using UnityEngine;

namespace Settings.Global.Audio
{
    public class AudioProviderMono : MonoBehaviour, IAudioProvider, IDisposable
    {
        #region Fields 

        private const string SoundSourcesContainerName = "SoundSourcesContainer";

        private MusicPlayer _musicPlayer;
        private SoundsPlayer _soundsPlayer;
        private GameObject _soundSourcesContainer;
        private AudioProviderSettings _settings;

        #endregion


        #region Properties 

        public MusicPlayer MusicPlayer => _musicPlayer;
        public SoundsPlayer SoundsPlayer => _soundsPlayer;


        #endregion


        #region Methods

        /// <summary>
        /// Initialize the audio provider component.
        /// </summary>
        /// <param name="soundAudioSourcesToCreate">target sound sources count for reuse (for simultaneous sounds playing) </param>
        /// <param name="externalMusicSource">Use it when you need the external music source (like from the main camera, etc.).</param>
        public void Initialize(AudioProviderSettings settings)
        {
            _settings = settings;
            CreateMusicPlayer(settings.MusicPlayer);
            CreateSoundsPlayer(_settings.SoundsPlayer);
        }

        /// <summary>
        /// Create music player
        /// </summary>
        /// <param name="musicSource">target music source, if it is null - create a new one on current gameObject</param>
        private void CreateMusicPlayer(MusicPlayerSettings settings)
        {
            _musicPlayer = new MusicPlayer(settings, this.transform);
        }

        private void CreateSoundsPlayer(SoundsPlayerSettings settings)
        {
            _soundSourcesContainer = new GameObject(SoundSourcesContainerName);
            _soundSourcesContainer.transform.SetParent(transform, false);
            _soundsPlayer = new SoundsPlayer(settings, _soundSourcesContainer.transform);
        }

        public void Dispose()
        {
            _musicPlayer?.Dispose();
            _soundsPlayer?.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }


#if UNITY_EDITOR

        //private void OnGUI()
        //{
        //    MusicPlayer.Debug.OnGUI();
        //}
#endif

        #endregion
    }
}
