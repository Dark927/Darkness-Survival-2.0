
using UnityEngine;

namespace UI.Audio
{
    [System.Serializable]
    public struct AudioEffectsSettingsUI
    {
        [SerializeField] private float _playDelay;
        [Space]
        [SerializeField] private SingleSFXSettings _hoverSFX;
        [SerializeField] private SingleSFXSettings _unhoverSFX;
        [SerializeField] private SingleSFXSettings _clickSFX;

        public float PlayDelay => _playDelay;
        public SingleSFXSettings HoverSFX => _hoverSFX;
        public SingleSFXSettings UnhoverSFX => _unhoverSFX;
        public SingleSFXSettings ClickSFX => _clickSFX;

        public static AudioEffectsSettingsUI FillEmpty(AudioEffectsSettingsUI destination, AudioEffectsSettingsUI src)
        {
            destination._hoverSFX = destination.HoverSFX.IsEmpty ? src.HoverSFX : destination.HoverSFX;
            destination._unhoverSFX = destination.UnhoverSFX.IsEmpty ? src.UnhoverSFX : destination.UnhoverSFX;
            destination._clickSFX = destination.ClickSFX.IsEmpty ? src.ClickSFX : destination.ClickSFX;

            destination._playDelay = destination.PlayDelay != 0 ? destination.PlayDelay : src.PlayDelay;

            return destination;
        }
    }
}
