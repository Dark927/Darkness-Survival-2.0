using System;
using UnityEngine;


namespace Materials.DarkEnemyFX
{
    [Serializable]
    public struct ParametricProps : IMaterialProps
    {
        [Range(0, 1)]
        public float _FlashAmount;
        [Range(0, 1)]
        public float _EmissionAmount;

        [Range(0.001f, 1f)]
        public float _Seed;
        [Range(0, 1)]
        public float _DissolveAmount;
        public bool _UseJitterFree;

        public DarkMainFX.RendererMode _RendMode;
        public bool NeedsUpdate { get; set; }
        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            mpb.SetFloat("_FlashAmount", _FlashAmount);
            mpb.SetFloat("_EmissionAmount", _EmissionAmount);

            mpb.SetFloat("_Seed", _Seed);
            mpb.SetFloat("_DissolveAmount", _DissolveAmount);

            if (_RendMode != DarkMainFX.RendererMode.IGNORE)
                mpb.SetInteger("_RendMode", (int)_RendMode | (_UseJitterFree ? 128 : 0));
        }
    }
}
