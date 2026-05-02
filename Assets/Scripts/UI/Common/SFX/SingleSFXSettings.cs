
using UnityEngine;

namespace UI.Audio
{
    [System.Serializable]
    public struct SingleSFXSettings
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volume;

        public AudioClip Clip => _clip;
        public float Volume => _volume;
        public bool IsEmpty => _clip == null;

    }
}
