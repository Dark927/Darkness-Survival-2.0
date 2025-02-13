
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Common.Features
{
    [CreateAssetMenu(fileName = "NewEntityFeaturesData", menuName = "Game/Characters/Data/EntityFeaturesData")]
    public class EntityFeaturesData : ScriptableObject
    {
        #region Fields 

        [SerializeField] private List<AssetReference> _featureAssetList;
        [SerializeField] private HashSet<AssetReference> _lol;

        #endregion


        #region Properties

        public List<AssetReference> FeatureAssetList => FilteredAssetReferences();

        #endregion


        #region Methods

        private List<AssetReference> FilteredAssetReferences()
        {
            return _featureAssetList
                .GroupBy(item => item?.RuntimeKey?.ToString())
                .Select(group => group.First())
                .ToList();
        }

        #endregion
    }
}