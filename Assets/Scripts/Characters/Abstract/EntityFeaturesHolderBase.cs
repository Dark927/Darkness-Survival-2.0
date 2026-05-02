using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Common
{
    public abstract class EntityFeaturesHolderBase<TFeature, TFeatureData> : IDisposable, IInitializable
        where TFeature : IDisposable
    {
        #region Fields 

        private Dictionary<int, TFeature> _featuresDict;
        private IEntityDynamicLogic _entityLogic;
        private GameObject _defaultFeaturesContainer;
        private string _targetContainerName;

        #endregion


        #region Properties

        public IEntityDynamicLogic EntityLogic => _entityLogic;
        public Dictionary<int, TFeature> ActiveOnesDict => _featuresDict;

        public GameObject DefaultFeaturesContainer => _defaultFeaturesContainer;
        public virtual string DefaultContainerName => $"{EntityLogic.Info.Name ?? "default"}_container";

        #endregion


        #region Methods

        protected abstract void DestroyFeatureLogic(TFeature feature);
        public abstract UniTask GiveAsync(TFeatureData featureData);


        #region Init

        public EntityFeaturesHolderBase(IEntityDynamicLogic targetEntity, string containerName = null)
        {
            _entityLogic = targetEntity;
            _featuresDict = new();
            _targetContainerName = containerName;
        }

        public virtual void Initialize()
        {

        }

        protected void TryInitContainer()
        {
            TryInitContainer(_targetContainerName);
        }

        protected virtual void TryInitContainer(string targetContainerName)
        {
            if (_defaultFeaturesContainer != null)
            {
                return;
            }

            string targetName = DefaultContainerName;

            if (!String.IsNullOrEmpty(targetContainerName))
            {
                targetName = targetContainerName;
            }

            _defaultFeaturesContainer = new GameObject(targetName);
            _defaultFeaturesContainer.transform.SetParent(EntityLogic.Body.Transform, false);
        }

        public virtual void Dispose()
        {
            RemoveAll();
        }

        #endregion

        public async UniTask GiveMultipleFeaturesAsync(IEnumerable<TFeatureData> featuresDataCollection)
        {
            List<UniTask> activeTasks = new List<UniTask>();

            UniTask currentTask;
            foreach (var featureData in featuresDataCollection)
            {
                currentTask = GiveAsync(featureData);
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
            foreach (var feature in _featuresDict.Values)
            {
                feature.Dispose();
                DestroyFeatureLogic(feature);
            }

            _featuresDict.Clear();
        }

        #endregion
    }
}
