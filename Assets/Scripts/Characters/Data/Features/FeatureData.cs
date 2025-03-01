
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Common.Features
{
    [CreateAssetMenu(fileName = "NewEntityFeaturesData", menuName = "Game/Characters/Features/Data/EntityFeatureData")]
    public class FeatureData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private AssetReferenceGameObject _assetRef;

        public string Name => _name;
        public AssetReferenceGameObject AssetRef => _assetRef;
    }
}
