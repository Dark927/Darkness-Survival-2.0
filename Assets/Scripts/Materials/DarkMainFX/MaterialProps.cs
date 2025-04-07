using System;
using UnityEngine;


namespace Materials.DarkMainFX
{
    [Serializable]
    public struct MaterialProps : IMaterialProps
    {
        private static readonly int s_rendModeID = Shader.PropertyToID("_RendMode");
        internal static readonly int s_fxFeaturesID = Shader.PropertyToID("_FXFeatures");

        public Features.Tint Tint;
        public Features.Flash Flash;
        public Features.Emission Emission;
        public Features.HSVReplace HSVParams;

        [Header("Mode")]
        public RendererMode RendMode;
        public bool UseJitterFree;

        public bool NeedsUpdate { get; set; }

        public readonly void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            var features = ShaderFeatures.NONE;

            features |= Flash.UpdateAllProperties(mpb);
            features |= Emission.UpdateAllProperties(mpb);
            features |= UseJitterFree ? ShaderFeatures.JITTERFREE : ShaderFeatures.NONE;
            features |= HSVParams.UpdateAllProperties(mpb);
            features |= Tint.UpdateAllProperties(mpb);

            mpb.SetInteger(s_rendModeID, (int)RendMode);
            mpb.SetInteger(s_fxFeaturesID, (int)features);
        }
    }
}
