using System;
using UnityEngine;


namespace Materials.DarkEntityFX
{
    [Serializable]
    public struct ParametricProps : IMaterialProps
    {
        [Range(0, 1)]
        public float FlashAmount;
        public Color FlashColor;
        [Range(0, 1)]
        public float EmissionAmount;

        [Range(0.001f, 1f)]
        public float Seed;
        [Range(0, 1)]
        public float DissolveAmount;
        public bool UseJitterFree;

        public DarkMainFX.RendererMode RendMode;
        public bool NeedsUpdate { get; set; }
        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            mpb.SetFloat("_FlashAmount", FlashAmount);
            mpb.SetColor("_FlashColor", FlashColor);
            mpb.SetFloat("_EmissionAmount", EmissionAmount);

            mpb.SetFloat("_Seed", Seed);
            mpb.SetFloat("_DissolveAmount", DissolveAmount);

            if (RendMode != DarkMainFX.RendererMode.IGNORE)
                mpb.SetInteger("_RendMode", (int)RendMode | (UseJitterFree ? 128 : 0));
        }
    }
}
