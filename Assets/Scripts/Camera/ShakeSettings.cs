using UnityEngine;

namespace Settings.CameraManagement.Shake
{
    [System.Serializable]
    public struct ShakeSettings
    {
        public static ShakeSettings Default = new ShakeSettings(1.0f);

        public const float DefaultAmplitude = 3.5f;
        public const float DefaultFrequency = 1.0f;

        [SerializeField] private float m_Amplitude;
        [SerializeField] private float m_Frequency;
        [SerializeField] private float m_Time;

        public float Amplitude => m_Amplitude;
        public float Frequency => m_Frequency;
        public float Time => m_Time;

        public ShakeSettings(float time, float amplitude = DefaultAmplitude, float frequency = DefaultFrequency)
        {
            m_Time = time;
            m_Amplitude = amplitude;
            m_Frequency = frequency;
        }

        public override string ToString()
        {
            return $"Amplitude : {Amplitude}, Frequency : {Frequency}, Time : {Time}.";
        }
    }
}
