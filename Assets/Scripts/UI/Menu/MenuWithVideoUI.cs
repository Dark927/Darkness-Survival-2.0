using System;
using System.Collections;
using Assets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI
{
    public class MenuWithVideoUI : FadeableColorUI<RawImage>, IMenuUI
    {
        [Header("Video Back - Transition Settings")]

        private VideoPlayer _videoPlayer;
        private RawImage _rawImage;


        protected virtual void Awake()
        {
            _videoPlayer = GetComponentInChildren<VideoPlayer>();
            _rawImage = GetComponentInChildren<RawImage>();
        }

        public virtual void Activate(Action callback = null)
        {
            ActivateVideoBackground();
            callback?.Invoke();
        }

        public virtual void Deactivate(Action callback = null, float speedMult = 1f)
        {
            DeactivateVideoBackground(speedMult);
            callback?.Invoke();
        }

        protected void ActivateVideoBackground()
        {
            _rawImage.color = Color.clear;

            _videoPlayer.prepareCompleted += VideoPlayerReady;
            _videoPlayer.Prepare();
        }

        protected virtual void VideoPlayerReady(VideoPlayer videoPlayer)
        {
            _videoPlayer.Play();
            Fade(_rawImage, Color.white);
        }

        protected virtual void DeactivateVideoBackground(float speedMult = 1f)
        {
            Fade(_rawImage, Color.clear, speedMult);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _videoPlayer.targetTexture.Release();
        }
    }
}
