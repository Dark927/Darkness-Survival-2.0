
namespace Settings.Global.Audio
{
    public class GameAudioService : IService, IAudioProvider
    {
        #region Fields 

        private IAudioProvider _audioProvider;

        #endregion


        #region Properties

        public MusicPlayer MusicPlayer => _audioProvider.MusicPlayer;
        public SoundsPlayer SoundsPlayer => _audioProvider.SoundsPlayer;

        #endregion


        #region Methods

        public GameAudioService(IAudioProvider audioProvider)
        {
            _audioProvider = audioProvider;
        }


        #endregion
    }
}
