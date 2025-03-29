using System;
using UnityEngine;


namespace Materials.DarkMainFX
{
    [Serializable]
    public struct ParametricProps : IMaterialProps
    {
        private static readonly int s_flashAmountID = Shader.PropertyToID("_FlashAmount");
        private static readonly int s_emissionAmountID = Shader.PropertyToID("_EmissionAmount");
        private static readonly int s_rendModeID = Shader.PropertyToID("_RendMode");

        [Range(0, 1)]
        public float FlashAmount;
        [Range(0, 1)]
        [SerializeField] public float EmissionAmount;
        public RendererMode RendMode;
        public bool UseJitterFree;

        public bool NeedsUpdate { get; set; }
        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            mpb.SetFloat(s_flashAmountID, FlashAmount);
            mpb.SetFloat(s_emissionAmountID, EmissionAmount);

            if (RendMode != RendererMode.IGNORE)
                mpb.SetInteger(s_rendModeID, (int)RendMode | (UseJitterFree ? 128 : 0));
        }
    }
}
