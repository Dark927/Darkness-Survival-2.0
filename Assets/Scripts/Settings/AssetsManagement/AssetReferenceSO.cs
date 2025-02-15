
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Settings.AssetsManagement
{
    [System.Serializable]
    public sealed class AssetReferenceSO : AssetReferenceT<ScriptableObject>
    {
        public AssetReferenceSO(string guid) : base(guid)
        {
        }
    }
}
