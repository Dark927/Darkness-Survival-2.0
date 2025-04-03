using UnityEngine;

namespace Settings.Global.Audio
{
    [System.Serializable]
    public struct MusicPlayerSettings
    {
        [SerializeField, Min(0)] private float _maxVolume;
        [SerializeField, Range(0f, 1.1f)] private float _reverbZoneMix;
        [SerializeField, Min(0)] private float _songTransitionTime;
        [SerializeField, Min(0)] private float _songInterruptionTransitionTime;

        public float MaxVolume => _maxVolume;
        public float ReverbZoneMix => _reverbZoneMix;
        public float SongTransitionTime => _songTransitionTime;
        public float SongInterruptionTransitionTime => _songInterruptionTransitionTime;

        public void SetMaxVolume(float maxVolume)
        {
            if (maxVolume > 0f)
            {
                _maxVolume = maxVolume;
            }
        }
    }
}
