using System;
using Materials.DarkMainFX;
using UnityEngine;


namespace Materials.Features
{
    [Serializable]
    public struct Tint : ITogglableMaterialProps
    {
        private static readonly int s_gamma = Shader.PropertyToID("_Gamma");
        private static readonly int s_color = Shader.PropertyToID("_Color");

        [Range(0, 3)] public float Gamma;
        [ColorUsage(true, true)] public Color Color;

        [SerializeField] private bool _enabled;
        public bool Enabled { readonly get => _enabled; set => _enabled = value; }


        public readonly ShaderFeatures UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (!Enabled) return ShaderFeatures.NONE;
            mpb.SetFloat(s_gamma, Gamma);
            mpb.SetColor(s_color, Color);
            return ShaderFeatures.TINT;
        }
    }
}
