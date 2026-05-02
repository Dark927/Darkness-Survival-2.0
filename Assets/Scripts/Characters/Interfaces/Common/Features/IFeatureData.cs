
using UnityEngine.AddressableAssets;

namespace Characters.Common.Features
{
    public interface IFeatureData
    {
        public int ID { get; }
        public string Name { get; }
        public AssetReferenceGameObject AssetRef { get; }
    }
}
