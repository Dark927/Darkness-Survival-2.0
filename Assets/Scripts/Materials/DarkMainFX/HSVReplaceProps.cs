using System;
using UnityEngine;


namespace Materials.DarkMainFX
{
    [Serializable]
    public struct HSVReplaceProps : IMaterialProps
    {
        [Header("HSV Replace")]
        [ColorUsage(false, false)]
        public Color _TargetColor;
        public bool _UseMask;

        [Space]
        [Range(0, 1f)] public float Hk;
        public float Sk;
        public float Vk;
        [Space]
        [Range(0, 1f)] public float Hb;
        public float Sb;
        public float Vb;

        [Header("Tolerances")]
        [Range(0, 0.5f)] public float _HueTolerance;
        [Range(0, 1f)] public float _SatTolerance;
        [Range(0, 1f)] public float _ValTolerance;

        [Header("Misc")]
        [Range(-1f, 5f)] public float _HSVGamma;

        public bool NeedsUpdate { get; set; }

        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            // ToDo : use ShaderID instead of strings
            mpb.SetColor("_TargetColor", _TargetColor);
            mpb.SetFloat("_UseMask", _UseMask ? 1f : 0f);
            mpb.SetVector("_ReplacementColorCoefK", new Vector4(Hk, Sk, Vk, 0));
            mpb.SetVector("_ReplacementColorCoefB", new Vector4(Hb, Sb, Vb, 0));

            mpb.SetFloat("_HueTolerance", _HueTolerance);
            mpb.SetFloat("_SatTolerance", _SatTolerance);
            mpb.SetFloat("_ValTolerance", _ValTolerance);

            mpb.SetFloat("_HSVGamma", _HSVGamma);
        }
    }

}
