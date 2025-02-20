using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Common
{
    public abstract class EntityFeaturesHolderBase<TFeature> : IDisposable, IInitializable where TFeature : IDisposable
    {
        #region Fields 

        private Dictionary<TFeature, AsyncOperationHandle> _loadedFeaturesDict;
        private IEntityDynamicLogic _entityLogic;

        #endregion


        #region Properties

        public IEntityDynamicLogic EntityLogic => _entityLogic;
        public Dictionary<TFeature, AsyncOperationHandle> LoadedFeaturesDict => _loadedFeaturesDict;

        #endregion


        #region Methods

        protected abstract void DestroyFeatureLogic(TFeature feature);
        public abstract UniTask GiveFeatureAsync<TFeatureData>(TFeatureData featureData);


        #region Init

        public EntityFeaturesHolderBase(IEntityDynamicLogic targetEntity)
        {
            _entityLogic = targetEntity;
            _loadedFeaturesDict = new Dictionary<TFeature, AsyncOperationHandle>();
        }

        public virtual void Initialize()
        {

        }

        public virtual void Dispose()
        {
            UnloadAll();
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

        public virtual void UnloadAll()
        {
            foreach (var loadedFeature in _loadedFeaturesDict)
            {
                loadedFeature.Key.Dispose();
                DestroyFeatureLogic(loadedFeature.Key);
                AddressableAssetsLoader.Instance.TryUnloadAsset(loadedFeature.Value);
            }

            _loadedFeaturesDict.Clear();
        }

        #endregion
    }
}
