
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities.Math;

namespace Characters.Common.Features
{
    [CreateAssetMenu(fileName = "NewEntityFeaturesData", menuName = "Game/Characters/Features/Data/EntityFeatureData")]
    public class FeatureData : ScriptableObject
    {
        private int _featureID;
        [SerializeField] private string _name;
        [SerializeField] private AssetReferenceGameObject _assetRef;

        public int ID => _featureID;
        public string Name => _name;
        public AssetReferenceGameObject AssetRef => _assetRef;


        private void Awake()
        {
            _featureID = CustomIdentifierGenerator.GenerateID(Name, AssetRef);
        }
    }
}
