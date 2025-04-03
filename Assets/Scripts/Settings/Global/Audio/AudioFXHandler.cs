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

        // Method to fade out the current music
        public void FadeOut(AudioSource musicSource, float fadeDuration, TweenCallback onCompleteCallback)
        {
            if (musicSource.isPlaying)
            {
                musicSource
                    .DOFade(0f, fadeDuration)
                    .SetEase(Ease.InOutCubic)
                    .OnComplete(onCompleteCallback)
                    .SetUpdate(true);
            }
            else
            {
                onCompleteCallback?.Invoke();
            }
        }


        public void FadeIn(AudioSource musicSource, float startVolume, float targetVolume, float fadeDuration)
        {
            if (musicSource.isPlaying)
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
