using UnityEngine;
using UnityEngine.UI;
using System;
using Settings.Global;
using Settings.Global.Audio;
using Utilities.Attributes;

namespace UI.SettingsMenu
{
    public class AudioSettingsPanelUI : SettingsPanelUI, IDisposable
    {
        [CustomHeader("Sliders", 3, 0)]
        [Tooltip("Ensure sliders are set to Min Value: 0.0001 and Max Value: 1.0")]
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private GameAudioService _audioService;

        public override void Initialize()
        {
            base.Initialize();

            _audioService = ServiceLocator.Current.Get<GameAudioService>();

            if (_audioService == null)
            {
                Debug.LogError("GameAudioService not found!");
                return;
            }

            // Set the initial slider positions based on the loaded JSON data
            _masterSlider.value = _audioService.SaveData.MasterVolume;
            _musicSlider.value = _audioService.SaveData.MusicVolume;
            _sfxSlider.value = _audioService.SaveData.SFXVolume;

            // Add the drag listeners
            _masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        private void OnMasterVolumeChanged(float value)
        {
            _audioService.SetVolume(AudioMixerParams.Master, ref _audioService.SaveData.MasterVolume, value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            _audioService.SetVolume(AudioMixerParams.Music, ref _audioService.SaveData.MusicVolume, value);
        }

        private void OnSFXVolumeChanged(float value)
        {
            _audioService.SetVolume(AudioMixerParams.SFX, ref _audioService.SaveData.SFXVolume, value);
        }

        // Save to disk ONLY when the player closes or navigates away from this UI panel
        private void OnDisable()
        {
            if (_audioService != null)
            {
                _audioService.SaveSettings();
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_masterSlider != null) _masterSlider.onValueChanged.RemoveAllListeners();
            if (_musicSlider != null) _musicSlider.onValueChanged.RemoveAllListeners();
            if (_sfxSlider != null) _sfxSlider.onValueChanged.RemoveAllListeners();
        }
    }
}
