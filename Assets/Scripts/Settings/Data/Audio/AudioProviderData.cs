
using UnityEngine;

namespace Settings.Global.Audio
{
    /// <summary>
    /// this SO provides the Audio Provider settings to configure music/sound players.
    /// </summary>
    [CreateAssetMenu(fileName = "NewAudioProviderData", menuName = "Game/Settings/Audio/Provider Data")]
    public class AudioProviderData : ScriptableObject
    {
        [SerializeField] private AudioProviderSettings _audioProviderSettings;

        public AudioProviderSettings Settings => _audioProviderSettings;
    }
}
