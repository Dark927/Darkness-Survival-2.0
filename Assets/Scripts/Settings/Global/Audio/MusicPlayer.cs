using UnityEngine;
using DG.Tweening;
using Utilities.UI;

namespace Settings.Global.Audio
{
    public class MusicPlayer
    {
        private AudioSource _musicSource;
        private Sequence _activeTransition;

        public MusicPlayer(AudioSource targetSource)
        {
            _musicSource = targetSource;
        }

        public void PlaySong(AudioClip targetClip, float transitionDuration = 1f)
        {
            TweenHelper.KillTweenIfActive(_activeTransition);

            Sequence songTransition = DOTween.Sequence();

            songTransition
                .Append(
                    _musicSource
                        .DOFade(0f, transitionDuration / 2f)
                        .From(_musicSource.volume)
                        .OnComplete(() =>
                        {
                            _musicSource.clip = targetClip;
                            _musicSource.Play();
                        })
                )
                .Append(
                    _musicSource
                        .DOFade(1f, transitionDuration / 2f)
                        .From(_musicSource.volume)
                );

            _activeTransition = songTransition;
        }
    }
}
