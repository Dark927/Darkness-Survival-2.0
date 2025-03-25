using System;
using UnityEngine;


namespace Materials.DarkEnemyFX
{
    [Serializable]
    public struct MaterialProps : IMaterialProps //todo: figure out classes
    {
        public DarkMainFX.MaterialProps _MainProps;

        [Header("Dissolve")]
        [Range(0, 5000)]
        public float _DissolveNoise;
        [ColorUsage(false, true)]
        public Color _DissolveColor;
        [Range(1, 2)]
        public float _DissolveOutline;

        public bool NeedsUpdate { get; set; }

        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {

            mpb.SetFloat("_DissolveNoise", _DissolveNoise);
            mpb.SetColor("_DissolveColor", _DissolveColor);
            mpb.SetFloat("_DissolveOutline", _DissolveOutline);

            _MainProps.UpdateAllProperties(mpb);
        }
    }
}
