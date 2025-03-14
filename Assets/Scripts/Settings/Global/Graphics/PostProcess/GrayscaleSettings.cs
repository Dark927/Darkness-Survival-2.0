
using UnityEngine;

namespace Settings.Global
{
    [System.Serializable]
    public struct GrayscaleSettings
    {
        [Range(0f, 100f)] public float Saturation;
        [Range(0f, 100f)] public float Contrast;
    }
}
