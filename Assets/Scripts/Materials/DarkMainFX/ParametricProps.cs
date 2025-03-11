using System;
using UnityEngine;


namespace Materials.DarkMainFX
{
    [Serializable]
    public struct ParametricProps : IMaterialProps
    {
        [Range(0, 1)]
        public float _FlashAmount;
        [Range(0, 1)]
        public float _EmissionAmount;
        public RendererMode _RendMode;
        public bool NeedsUpdate { get; set; }
        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            mpb.SetFloat("_FlashAmount", _FlashAmount);
            mpb.SetFloat("_EmissionAmount", _EmissionAmount);

            if (_RendMode != RendererMode.IGNORE)
                mpb.SetInteger("_RendMode", (int)_RendMode);
        }
    }
}
