
using Settings;
using UnityEngine;

namespace UI.Audio
{
    [CreateAssetMenu(fileName = "UI_NewAudioEffects_Data", menuName = "Game/UI/Audio/Audio Effects Data")]
    public class AudioEffectsDataUI : DescriptionBaseData
    {
        [SerializeField] private AudioEffectsSettingsUI _audioEffectsSettings;

        public AudioEffectsSettingsUI Effects => _audioEffectsSettings;
    }
}
