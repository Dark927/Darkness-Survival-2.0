
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Settings.Global.Audio
{
    // A dedicated class for Mixer string parameters
    public static class AudioMixerParams
    {
        public const string Master = "MasterVolume";
        public const string Music = "MusicVolume";
        public const string SFX = "SFXVolume";
    }

    public class GameAudioService : IService, IAudioProvider, IInitializable
    {
        #region Fields 

        private IAudioProvider _audioProvider;
        private AudioMixer _mainMixer;
        private ISettingsStorage _settingsStorage;

        #endregion


        #region Properties

        public MusicPlayer MusicPlayer => _audioProvider.MusicPlayer;
        public SoundsPlayer SoundsPlayer => _audioProvider.SoundsPlayer;
        public AudioSaveData SaveData => _settingsStorage.Data.Audio;

        #endregion


        #region Methods

        public GameAudioService(IAudioProvider audioProvider, AudioMixer mainMixer, ISettingsStorage settingsStorage)
        {
            _audioProvider = audioProvider;
            _mainMixer = mainMixer;
            _settingsStorage = settingsStorage;
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            await UniTask.WaitUntil(() => _settingsStorage.IsLoaded);
            ApplyAllVolumes();
        }

        public void SetVolume(string exposedParameter, ref float savedValue, float sliderValue)
        {
            savedValue = sliderValue;
            _mainMixer.SetFloat(exposedParameter, ConvertLinearValueToDb(sliderValue));
        }

        public void SaveSettings()
        {
            _settingsStorage.SaveAllSettings();
        }

        private void ApplyAllVolumes()
        {
            SetVolume(AudioMixerParams.Master, ref SaveData.MasterVolume, SaveData.MasterVolume);
            SetVolume(AudioMixerParams.Music, ref SaveData.MusicVolume, SaveData.MusicVolume);
            SetVolume(AudioMixerParams.SFX, ref SaveData.SFXVolume, SaveData.SFXVolume);
        }

        // Convert Linear value (0.0001 to 1) to Logarithmic Decibels (-80dB to 0dB)
        private float ConvertLinearValueToDb(float linearValue)
        {
            float clampedValue = Mathf.Max(0.0001f, linearValue);
            return Mathf.Log10(clampedValue) * 20f;
        }

        #endregion
    }
}
