using System;
using Materials.DarkMainFX;
using Materials;
using UnityEngine;



namespace Materials.Features
{

    [Serializable]
    public struct SimpleHSV : ITogglableMaterialProps
    {
        [Range(0, 1f)] public float Hk;
        public float Sk;
        public float Vk;
        [Space]
        [Range(0, 1f)] public float Hb;
        public float Sb;
        public float Vb;

        [SerializeField] private bool _enabled;
        public bool Enabled { readonly get => _enabled; set => _enabled = value; }

        public readonly ShaderFeatures UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (!Enabled) return ShaderFeatures.NONE;
            var matrix = Matrix4x4.zero;
            matrix.m00 = Hk;
            matrix.m11 = Sk;
            matrix.m22 = Vk;
            matrix.m33 = 1f;
            mpb.SetMatrix(HSVReplace.s_hsvaKID, matrix);
            mpb.SetVector(HSVReplace.s_hsvaVID, new Vector4(Hb, Sb, Vb, 0f));
            return ShaderFeatures.NONE;
        }
    }


    [Serializable]
    public struct MatrixHSVA : ITogglableMaterialProps
    {
        [MatrixLabels("Hk", "Sk", "Vk", "Ak")]
        public Matrix4x4 HsvaCoefs;
        [MatrixLabels("Hb", "Sb", "Vb", "Ab")]
        public Vector4 HsvaBias;

        [SerializeField] private bool _enabled;
        public bool Enabled { readonly get => _enabled; set => _enabled = value; }

        public readonly ShaderFeatures UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (!Enabled) return ShaderFeatures.NONE;
            mpb.SetMatrix(HSVReplace.s_hsvaKID, HsvaCoefs);
            mpb.SetVector(HSVReplace.s_hsvaVID, HsvaBias);
            return ShaderFeatures.NONE;
        }
    }


    [Serializable]
    public struct HSVReplace : ITogglableMaterialProps
    {
        private static readonly int s_targetColorID = Shader.PropertyToID("_TargetColor");
        private static readonly int s_useMaskID = Shader.PropertyToID("_UseMask");
        private static readonly int s_hueToleranceID = Shader.PropertyToID("_HueTolerance");
        private static readonly int s_satToleranceID = Shader.PropertyToID("_SatTolerance");
        private static readonly int s_valToleranceID = Shader.PropertyToID("_ValTolerance");
        private static readonly int s_hSVGammaID = Shader.PropertyToID("_HSVGamma");

        internal static readonly int s_hsvaKID = Shader.PropertyToID("_HsvaK");
        internal static readonly int s_hsvaVID = Shader.PropertyToID("_HsvaB");




        [Header("HSV Replace")]
        [ColorUsage(false, false)]
        public Color TargetColor;
        [Header("_HSVMaskTex")]
        public bool UseMask;

        public SimpleHSV SimpleHSV;
        public MatrixHSVA MatrixHSVA;

        [Header("Tolerances")]
        [Range(0, 0.5f)] public float _HueTolerance;
        [Range(0, 1f)] public float _SatTolerance;
        [Range(0, 1f)] public float _ValTolerance;

        [Header("Misc")]
        [Range(-1f, 5f)] public float _HSVGamma;

        [SerializeField] private bool _enabled;
        public bool Enabled { readonly get => _enabled; set => _enabled = value; }

        public readonly ShaderFeatures UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (!Enabled) return ShaderFeatures.NONE;

            // ToDo : use ShaderID instead of strings
            mpb.SetColor(s_targetColorID, TargetColor);
            mpb.SetFloat(s_useMaskID, UseMask ? 1f : 0f);

            SimpleHSV.UpdateAllProperties(mpb);
            MatrixHSVA.UpdateAllProperties(mpb);

            mpb.SetFloat(s_hueToleranceID, _HueTolerance);
            mpb.SetFloat(s_satToleranceID, _SatTolerance);
            mpb.SetFloat(s_valToleranceID, _ValTolerance);

            mpb.SetFloat(s_hSVGammaID, _HSVGamma);
            return ShaderFeatures.HSVREPLACE;
        }
    }

}
