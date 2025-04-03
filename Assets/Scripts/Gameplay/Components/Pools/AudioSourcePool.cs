

using Settings.Global.Audio;
using UnityEngine;

namespace Gameplay.Components
{
    public class AudioSourcePool : ComponentsPoolBase<AudioSource>
    {
        private SoundsPlayerSettings _settings;

        public AudioSourcePool(SoundsPlayerSettings settings, GameObject poolItemPrefab, Transform container) : base(settings.SoundSourcesPoolSettings, poolItemPrefab, container)
        {
            _settings = settings;
        }

        protected override AudioSource PreloadFunc(Transform container = null)
        {
            AudioSource createdSource = base.PreloadFunc(container);
            ConfigureSoundSource(createdSource, _settings);
            return createdSource;
        }

        private void ConfigureSoundSource(AudioSource source, SoundsPlayerSettings settings)
        {
            source.transform.position = new Vector3(0f, 0f, settings.PositionOffsetZ);
            source.volume = settings.DefaultVolume;
            source.rolloffMode = AudioRolloffMode.Custom;
            source.spatialBlend = settings.SpatialBlend;
            source.minDistance = settings.MinDistance;    // The distance at which the sound will be at its full volume
            source.maxDistance = settings.MaxDistance;    // The maximum distance where the sound will fade out
            source.playOnAwake = false;
            source.loop = false;
        }

        protected override void RequestAction(AudioSource source)
        {
            base.RequestAction(source);
            source.gameObject.SetActive(true);
        }

        protected override void ReturnAction(AudioSource source)
        {
            base.ReturnAction(source);
            ConfigureSoundSource(source, _settings);
            source.gameObject.SetActive(false);
        }
    }

}

