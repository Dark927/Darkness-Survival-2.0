
using UnityEngine;

namespace Materials
{
    public interface ITogglableMaterialProps
    {
        // Warning: implementers should have private bool _enabled defined
        bool Enabled { get; set; }
        // Togglable properties always need updates. returns a feature taht needs to be ORed
        DarkMainFX.ShaderFeatures UpdateAllProperties(MaterialPropertyBlock mpb);
    }
}
