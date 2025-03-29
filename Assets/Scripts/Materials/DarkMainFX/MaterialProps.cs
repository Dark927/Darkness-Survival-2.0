using System;
using UnityEngine;


namespace Materials.DarkMainFX
{
    [Serializable]
    public struct MaterialProps : IMaterialProps
    {
        private static readonly int s_gammaID = Shader.PropertyToID("_Gamma");
        private static readonly int s_colorID = Shader.PropertyToID("_Color");

        private static readonly int s_flashAmountID = Shader.PropertyToID("_FlashAmount");
        private static readonly int s_flashColorID = Shader.PropertyToID("_FlashColor");

        private static readonly int s_emissionColorID = Shader.PropertyToID("_EmissionColor");
        private static readonly int s_emissionAmountID = Shader.PropertyToID("_EmissionAmount");
        private static readonly int s_rendModeID = Shader.PropertyToID("_RendMode");

        [Header("Color")]
        [Range(0, 3)]
        public float _Gamma;
        public Color _ColorTint;

        [Header("Flash")]
        [ColorUsage(false, false)]
        public Color _FlashColor;
        [Range(0, 1)]
        [SerializeField] private float _FlashAmount;


        public float FlashAmount
        {
            get => _FlashAmount;
            set
            {
                _FlashAmount = value;
                NeedsUpdate = true;
            }
        }


        [Header("Emission")]
        [ColorUsage(true, true)]
        public Color _EmissionColor;

        [Range(0, 1)]
        public float _EmissionAmount;

        public HSVReplaceProps _HSVParams;

        [Header("Mode")]
        public RendererMode _RendMode;
        public bool _UseJitterFree;


        public bool NeedsUpdate { get; set; }

        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            mpb.SetFloat(s_gammaID, _Gamma);
            mpb.SetColor(s_colorID, _ColorTint);

            mpb.SetColor(s_flashColorID, _FlashColor);
            mpb.SetFloat(s_flashAmountID, _FlashAmount);

            mpb.SetColor(s_emissionColorID, _EmissionColor);
            mpb.SetFloat(s_emissionAmountID, _EmissionAmount);

            mpb.SetInteger(s_rendModeID, (int)_RendMode | (_UseJitterFree ? 128 : 0));

            _HSVParams.UpdateAllProperties(mpb);
        }
    }
}
