using System;
using Dark.Environment;
using UnityEngine;


namespace Materials.Shadow
{
    [Serializable]
    public class ShadowMaterialProps : IMaterialProps
    {
        //[HideInInspector]
        public float TimeMultiplier;
        public Color ShadowColor = new(0, 0, 0, 0.2f);
        public float SunDirection;
        public float PerspectiveStrength = 0.1f;
        public Vector3 ShadowScale = new(0, 0.1f, 0);
        public Vector3 RootPos;

        public DayManager? _dayManager;

        public bool NeedsUpdate { get => true; set { } }

        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (_dayManager == null)
            {
                Debug.LogError("Timer is null!");
                return;
            }

            /// 0 - 12pm day
            /// 0.5 - 12am night
            /// 1 - 12pm day again
            mpb.SetFloat("_TimeFactor", _dayManager.InGameTime);

            mpb.SetColor("_ShadowColorDay", ShadowColor);
            mpb.SetColor("_ShadowColorSunset", ShadowColor);

            mpb.SetFloat("_SunDirection", SunDirection);
            mpb.SetFloat("_PerspectiveStrength", PerspectiveStrength);
            mpb.SetVector("_ShadowScale", ShadowScale);

            mpb.SetVector("_RootPos", RootPos);
        }
    }
}
