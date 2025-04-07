using System;
using Materials.DarkMainFX;
using UnityEngine;


namespace Materials.DarkEntityFX
{
    [Serializable]
    public struct MaterialProps : IMaterialProps
    {
        public DarkMainFX.MaterialProps MainProps;

        public Features.Dissolve Dissolve;

        public readonly bool NeedsUpdate { get => true; set { } }
        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {

            MainProps.UpdateAllProperties(mpb);
            var features = mpb.GetInteger(DarkMainFX.MaterialProps.s_fxFeaturesID);
            features |= (int)Dissolve.UpdateAllProperties(mpb);
            mpb.SetInteger(DarkMainFX.MaterialProps.s_fxFeaturesID, features);
        }
    }
}
