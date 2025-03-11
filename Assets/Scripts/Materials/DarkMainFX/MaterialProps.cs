using System;
using UnityEngine;


namespace Materials.DarkMainFX
{
    [Serializable]
    public struct MaterialProps : IMaterialProps //todo: figure out classes
    {
        [Header("Color")]
        [Range(0, 3)]
        public float _Gamma;
        public Color _ColorTint;

        [Header("Flash")]
        [ColorUsage(false, false)]
        public Color _FlashColor;
        [Range(0, 1)]
        public float _FlashAmount;



        [Header("Emission")]
        [ColorUsage(true, true)]
        public Color _EmissionColor;

        [Range(0, 1)]
        public float _EmissionAmount;

        public HSVReplaceProps _HSVParams;

        [Header("Mode")]
        public RendererMode _RendMode;

        public bool NeedsUpdate { get; set; }

        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            mpb.SetFloat("_Gamma", _Gamma);
            mpb.SetColor("_Color", _ColorTint);

            mpb.SetColor("_FlashColor", _FlashColor);
            mpb.SetFloat("_FlashAmount", _FlashAmount);

            mpb.SetColor("_EmissionColor", _EmissionColor);
            mpb.SetFloat("_EmissionAmount", _EmissionAmount);

            mpb.SetInteger("_RendMode", (int)_RendMode);

            _HSVParams.UpdateAllProperties(mpb);
        }
    }
}
