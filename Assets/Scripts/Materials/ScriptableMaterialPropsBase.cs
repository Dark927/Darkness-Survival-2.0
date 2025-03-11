using System;
using UnityEngine;

namespace Materials
{

    [Serializable]
    public abstract class ScriptableMaterialPropsBase : ScriptableObject, IMaterialProps
    {
        public bool NeedsUpdate { get; set; } = true;

        public abstract void UpdateAllProperties(MaterialPropertyBlock mpb);
    }
}
