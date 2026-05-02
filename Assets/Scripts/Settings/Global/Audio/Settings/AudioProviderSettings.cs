using UnityEngine;
using Utilities.Attributes;

namespace Settings.Global.Audio
{
    [System.Serializable]
    public struct AudioProviderSettings
    {
        [CustomHeader("Music Player - Settings")]
        [SerializeField] private MusicPlayerSettings _musicPlayerSettings;

        [CustomHeader("Sounds Player - Settings")]
        [SerializeField] private SoundsPlayerSettings _soundsPlayerSettings;


        public MusicPlayerSettings MusicPlayer => _musicPlayerSettings;
        public SoundsPlayerSettings SoundsPlayer => _soundsPlayerSettings;
    }
}
