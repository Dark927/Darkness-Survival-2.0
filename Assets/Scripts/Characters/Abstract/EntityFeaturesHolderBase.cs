using System;
using System.Collections.Generic;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Common
{
    public abstract class EntityFeaturesHolderBase<TFeature> : IDisposable, IInitializable where TFeature : IDisposable
    {
        #region Fields 

        private Dictionary<int, TFeature> _featuresDict;
        private IEntityDynamicLogic _entityLogic;

        #endregion


        #region Properties

        public IEntityDynamicLogic EntityLogic => _entityLogic;
        public Dictionary<int, TFeature> ActiveOnesDict => _featuresDict;

        #endregion


        #region Methods

        protected abstract void DestroyFeatureLogic(TFeature feature);
        public abstract UniTask GiveFeatureAsync<TFeatureData>(TFeatureData featureData);


        #region Init

        public EntityFeaturesHolderBase(IEntityDynamicLogic targetEntity)
        {
            _entityLogic = targetEntity;
            _featuresDict = new();
        }

        public virtual void Initialize()
        {

        }

        public virtual void Dispose()
        {
            RemoveAll();
        }

        #endregion

        public async UniTask GiveMultipleFeaturesAsync<TFeatureData>(IEnumerable<TFeatureData> featuresDataCollection) where TFeatureData : ScriptableObject
        {
            List<UniTask> activeTasks = new List<UniTask>();

            UniTask currentTask;
            foreach (var featureData in featuresDataCollection)
            {
                currentTask = GiveFeatureAsync(featureData);
                activeTasks.Add(currentTask);
            }

            await UniTask.WhenAll(activeTasks);
        }

        protected virtual TFeature CreateFeature(GameObject featureAssetObj, string featureName, Transform parent)
        {
            var featureObj = GameObject.Instantiate(featureAssetObj);
            featureObj.name = featureName;
            featureObj.transform.SetParent(parent, false);
            return featureObj.GetComponent<TFeature>();
        }

        public virtual void RemoveAll()
        {
            foreach (var feature in _featuresDict)
            {
                feature.Value.Dispose();
                DestroyFeatureLogic(feature.Value);
            }

            _featuresDict.Clear();
        }

        #endregion
    }
}
