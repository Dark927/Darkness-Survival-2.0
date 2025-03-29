using System;
using UnityEngine;


namespace Materials.DarkEntityFX
{
    [Serializable]
    public struct MaterialProps : IMaterialProps
    {
        private static readonly int s_dissolveNoiseID = Shader.PropertyToID("_DissolveNoise");
        private static readonly int s_dissolveColorID = Shader.PropertyToID("_DissolveColor");
        private static readonly int s_dissolveOutlineID = Shader.PropertyToID("_DissolveOutline");

        public DarkMainFX.MaterialProps _MainProps;

        [Header("Dissolve")]
        [Range(0, 5000)]
        public float DissolveNoise;
        [ColorUsage(false, true)]
        public Color DissolveColor;
        [Range(1, 2)]
        public float DissolveOutline;
        public bool NeedsUpdate { get; set; }

        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            mpb.SetFloat(s_dissolveNoiseID, DissolveNoise);
            mpb.SetColor(s_dissolveColorID, DissolveColor);
            mpb.SetFloat(s_dissolveOutlineID, DissolveOutline);

            _MainProps.UpdateAllProperties(mpb);
        }
    }
}
