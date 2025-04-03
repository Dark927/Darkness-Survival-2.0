using UnityEngine;
using Cysharp.Threading.Tasks;
using Utilities.ErrorHandling;
using Gameplay.Components;

namespace Settings.Global.Audio
{
    public class SoundsPlayer
    {
        public Vector2 DefaultSoundPosition => Vector2.zero;
        private SoundsPlayerSettings _settings;
        private AudioSourcePool _pool;

        public SoundsPlayer(SoundsPlayerSettings settings, Transform audioSourcesContainer)
        {
            _settings = settings;
            _pool = new AudioSourcePool(settings, null, audioSourcesContainer);
            _pool.Initialize();
        }

        public void PlaySound(AudioClip sound)
        {
            PlaySound(sound, DefaultSoundPosition);
        }

        public void PlaySound(AudioClip sound, Vector2 position, float? maxDistance = null)
        {
            PlayOneShot(sound, position, maxDistance);
        }

        private void PlayOneShot(AudioClip sound, Vector2 position, float? maxDistance = null)
        {
            var source = _pool.RequestObject();

            if (source != null)
            {
                source.transform.position = new Vector3(position.x, position.y, source.transform.position.z);

                if (maxDistance != null)
                {
                    source.maxDistance = maxDistance.Value;
                }

                source.PlayOneShot(sound);
                ReturnAudioSourceAfterFinish(source).Forget();
            }
            else
            {
                ErrorLogger.LogWarning("All audio sources are currently playing.");
            }
        }

        private async UniTask ReturnAudioSourceAfterFinish(AudioSource source)
        {
            while (source.isPlaying)
            {
                await UniTask.Yield();
            }

            _pool.ReturnItem(source);
        }
    }
}
