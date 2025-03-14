using System;
using UnityEngine.Video;

namespace UI
{
    public class MenuWithVideoUI : BasicMenuUI
    {
        private VideoPlayer _videoPlayer;

        protected override void Awake()
        {
            base.Awake();
            _videoPlayer = GetComponentInChildren<VideoPlayer>();
        }

        public override void Activate(Action callback = null)
        {
            base.Activate(callback);
            ActivateVideo();
        }

        protected override void Deactivate(Action callerCallback = null, Action ownerCloseCallback = null, float speedMult = 1)
        {
            base.Deactivate(callerCallback, ownerCloseCallback, speedMult);
            DeactivateVideo(speedMult);
        }

        protected void ActivateVideo()
        {
            _videoPlayer.prepareCompleted += VideoPlayerReady;
            _videoPlayer.Prepare();
        }

        protected virtual void VideoPlayerReady(VideoPlayer videoPlayer)
        {
            _videoPlayer.Play();
        }

        protected virtual void DeactivateVideo(float speedMult = 1f)
        {
            _videoPlayer.Stop();
        }

        private void OnDestroy()
        {
            _videoPlayer.targetTexture.Release();
        }
    }
}
