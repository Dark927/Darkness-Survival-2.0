
using System;
using Settings.Global;
using Settings.Global.Audio;

namespace UI
{
    public class MainMenuUI : MenuWithVideoUI
    {
        private GameAudioService _audioService;

        private void Start()
        {
            Activate();
        }

        public override void Activate(Action callback = null)
        {
            base.Activate(callback);
            _audioService = ServiceLocator.Current.Get<GameAudioService>();
            _audioService.MusicPlayer.StartPlaylist(MusicType.MainMenu);
        }
    }
}
