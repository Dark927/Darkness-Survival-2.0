using UnityEngine;
using Cysharp.Threading.Tasks;
using Utilities.ErrorHandling;
using Gameplay.Components;
using System.Threading;
using System;
using UI.Audio;
using static Unity.VisualScripting.Member;

namespace Settings.Global.Audio
{
    public class SoundsPlayer : IDisposable
    {
        #region Fields 

        private const short UseDefaultVolumeIdentifier = -1;
        private SoundsPlayerSettings _settings;
        private AudioSourcePool _pool;
        private CancellationTokenSource _cts;

        #endregion


        #region Properties

        public Vector2 DefaultSoundPosition => Vector2.zero;

        #endregion


        #region Methods

        #region Init 

        public SoundsPlayer(SoundsPlayerSettings settings, Transform audioSourcesContainer)
        {
            _settings = settings;
            _pool = new AudioSourcePool(settings, null, audioSourcesContainer);
            _pool.Initialize();
            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        #endregion


        #region SingleSFX Workflow

        public void Play3DSound(SingleSFXSettings sfx, Vector2? position = null, float? maxDistance = null)
        {
            Play3DSound(sfx.Clip, sfx.Volume, position, maxDistance);
        }

        public void Play2DSound(SingleSFXSettings sfx)
        {
            Play2DSound(sfx.Clip, sfx.Volume);
        }

        #endregion


        #region AudioClip Workflow

        public void Play3DSound(AudioClip targetSound, float volume = UseDefaultVolumeIdentifier, Vector2? position = null, float? maxDistance = null)
        {
            PlaySoundOneShot(targetSound, (source) =>
            {
                Configure3DSource(source, volume, position, maxDistance);
            });
        }

        public void Play2DSound(AudioClip targetSound, float volume = UseDefaultVolumeIdentifier)
        {
            PlaySoundOneShot(targetSound, (source) =>
            {
                Configure2DSource(source, volume);
            });
        }

        #endregion

        private void PlaySoundOneShot(AudioClip sound, Action<AudioSource> sourceConfigurationLogic)
        {
            if (sound == null)
            {
                return;
            }

            var source = _pool.RequestObject();

            if (source != null)
            {
                sourceConfigurationLogic(source);
                source.PlayOneShot(sound);

                if (_cts == null || _cts.IsCancellationRequested)
                {
                    _cts = new CancellationTokenSource();
                }

                ReturnAudioSourceAfterFinish(source, _cts.Token).Forget();
            }
            else
            {
                ErrorLogger.LogWarning("All audio sources are currently playing.");
            }
        }

        private void Configure3DSource(AudioSource source, float volume, Vector2? position = null, float? maxDistance = null)
        {
            // Position 

            Vector3 targetPosition = Vector2.zero;

            if (position != null)
            {
                targetPosition = position.Value;
            }

            targetPosition.z = source.transform.position.z;

            source.transform.position = targetPosition;

            ConfigureSourceVolume(source, volume);

            // Max Distance 

            if (maxDistance != null)
            {
                source.maxDistance = maxDistance.Value;
            }

        }

        private static void ConfigureSourceVolume(AudioSource source, float volume)
        {
            if (volume != UseDefaultVolumeIdentifier)
            {
                source.volume = volume;
            }
        }

        private void Configure2DSource(AudioSource source, float volume)
        {
            ConfigureSourceVolume(source, volume);
            source.spatialBlend = 0f;
        }

        private async UniTask ReturnAudioSourceAfterFinish(AudioSource source, CancellationToken token = default)
        {
            while (source.isPlaying)
            {
                await UniTask.Yield(token);

                if (token.IsCancellationRequested)
                {
                    return;
                }
            }

            _pool.ReturnItem(source);
        }

        #endregion
    }
}
