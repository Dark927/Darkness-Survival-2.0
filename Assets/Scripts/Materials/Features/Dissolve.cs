using System;
using Materials.DarkMainFX;
using UnityEngine;


namespace Materials.Features
{
    [Serializable]
    public struct Dissolve : ITogglableMaterialProps
    {
        private static readonly int s_dissolveNoiseID = Shader.PropertyToID("_DissolveNoise");
        private static readonly int s_dissolveColorID = Shader.PropertyToID("_DissolveColor");
        private static readonly int s_dissolveOutlineID = Shader.PropertyToID("_DissolveOutline");

        [Range(0, 5000)] public float DissolveNoise;
        [ColorUsage(false, true)] public Color DissolveColor;
        [Range(1, 2)] public float DissolveOutline;

        [SerializeField] private bool _enabled;
        public bool Enabled { readonly get => _enabled; set => _enabled = value; }


        public readonly ShaderFeatures UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (!Enabled) return ShaderFeatures.NONE;
            mpb.SetFloat(s_dissolveNoiseID, DissolveNoise);
            mpb.SetColor(s_dissolveColorID, DissolveColor);
            mpb.SetFloat(s_dissolveOutlineID, DissolveOutline);
            return ShaderFeatures.DISSOLVE;
        }
    }
}
