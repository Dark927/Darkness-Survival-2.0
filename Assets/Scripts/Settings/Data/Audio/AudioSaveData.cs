using System;

namespace Settings.Global.Audio
{
    [Serializable]
    public class AudioSaveData
    {
        public float MasterVolume = 1f;
        public float MusicVolume = 1f;
        public float SFXVolume = 1f;
    }
}
