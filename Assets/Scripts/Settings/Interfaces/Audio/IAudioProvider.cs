

namespace Settings.Global.Audio
{
    public interface IAudioProvider
    {
        public MusicPlayer MusicPlayer { get; }
        public SoundsPlayer SoundsPlayer { get; }
    }
}
