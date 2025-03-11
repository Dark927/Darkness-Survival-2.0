
using UnityEngine;

namespace Materials
{
    public interface IMaterialProps
    {
        bool NeedsUpdate { get; set; }
        void UpdateAllProperties(MaterialPropertyBlock mpb);
    }
}
