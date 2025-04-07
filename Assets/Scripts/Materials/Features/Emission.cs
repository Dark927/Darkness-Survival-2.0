using System;
using Materials.DarkMainFX;
using UnityEngine;


namespace Materials.Features
{
    [Serializable]
    public struct Emission : ITogglableMaterialProps
    {
        private static readonly int s_emissionColorID = Shader.PropertyToID("_EmissionColor");
        private static readonly int s_emissionAmountID = Shader.PropertyToID("_EmissionAmount");

        [ColorUsage(true, true)] public Color EmissionColor;
        [Range(0, 1)] public float EmissionAmount;
        [Header("Use _EmissionMaskTex")]
        public bool UseTexture;

        [SerializeField] private bool _enabled;
        public bool Enabled { readonly get => _enabled; set => _enabled = value; }

        public readonly ShaderFeatures UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (!Enabled) return ShaderFeatures.NONE;
            mpb.SetColor(s_emissionColorID, EmissionColor);
            mpb.SetFloat(s_emissionAmountID, EmissionAmount);
            return ShaderFeatures.EMISSION;
        }
    }
}
