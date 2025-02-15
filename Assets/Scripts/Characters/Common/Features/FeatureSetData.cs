
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Common.Features
{
    [CreateAssetMenu(fileName = "NewEntityFeaturesData", menuName = "Game/Characters/Features/Data/FeatureSetData")]
    public class FeatureSetData : ScriptableObject
    {
        #region Fields 

        [SerializeField] private List<FeatureData> _featureAssetList;

        #endregion


        #region Properties

        public List<FeatureData> FeatureAssetList => FilteredAssetReferences();

        #endregion


        #region Methods

        private List<FeatureData> FilteredAssetReferences()
        {
            return _featureAssetList
                .GroupBy(item => item?.AssetRef?.RuntimeKey?.ToString())
                .Select(group => group.First())
                .ToList();
        }

        #endregion
    }
}