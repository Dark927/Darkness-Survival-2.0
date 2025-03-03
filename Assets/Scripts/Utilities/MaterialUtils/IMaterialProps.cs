
using UnityEngine;

namespace Utilities
{
    public interface IMaterialProps
    {
        bool NeedsUpdate { get; set; }
        void UpdateAllProperties(MaterialPropertyBlock mpb);
    }
}
