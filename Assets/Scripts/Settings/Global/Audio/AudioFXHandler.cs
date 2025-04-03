using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Settings.Global.Audio
{
    public class AudioFXHandler
    {
        #region Fade Effects 

        public bool TryFadeOutPlayingSong(AudioSource musicSource, float fadeDuration, out Tween animation)
        {
            animation = null;

            if (musicSource != null && musicSource.isPlaying)
            {
                animation = musicSource
                                .DOFade(0f, fadeDuration)
                                .SetEase(Ease.InOutCubic)
                                .SetUpdate(true);

                return true;
            }

            return false;
        }

        // Method to fade out the audio source
        public Tween FadeOut(AudioSource musicSource, float fadeDuration, Action onCompleteCallback = null)
        {
            return musicSource
                .DOFade(0f, fadeDuration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => onCompleteCallback?.Invoke())
                .SetUpdate(true);
        }


        public void FadeIn(AudioSource musicSource, float startVolume, float targetVolume, float fadeDuration)
        {
            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource
                    .DOFade(targetVolume, fadeDuration)
                    .From(startVolume)
                    .SetEase(Ease.InOutCubic)
                    .SetUpdate(true);
            }
        }

        #endregion
    }
}
