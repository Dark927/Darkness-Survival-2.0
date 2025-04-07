using System;
using Materials.DarkMainFX;
using UnityEngine;


namespace Materials.Features
{
    [Serializable]
    public struct Flash : ITogglableMaterialProps
    {
        private static readonly int s_flashAmountID = Shader.PropertyToID("_FlashAmount");
        private static readonly int s_flashColorID = Shader.PropertyToID("_FlashColor");


        [ColorUsage(false, false)] public Color FlashColor;
        [Range(0, 1)] public float FlashAmount;

        [SerializeField] private bool _enabled;
        public bool Enabled { readonly get => _enabled; set => _enabled = value; }

        public readonly ShaderFeatures UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (!Enabled) return ShaderFeatures.NONE;
            mpb.SetColor(s_flashColorID, FlashColor);
            mpb.SetFloat(s_flashAmountID, FlashAmount);
            return ShaderFeatures.FLASH;
        }
    }
}
