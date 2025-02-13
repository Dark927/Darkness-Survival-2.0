using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable enable

namespace Characters.Common.Features
{
    public class EntityFeaturesHolder : IDisposable
    {
        #region Fields 

        private IEntityLogic _entityLogic;
        private List<IEntityFeature> _featuresList;
        private EntityFeaturesData _featuresData;
        private bool _isFeaturesLoaded = false;

        private List<AsyncOperationHandle<GameObject>> _loadedFeaturesHandles = new();

        #endregion


        #region Methods

        public EntityFeaturesHolder(IEntityLogic targetCharacter, EntityFeaturesData data)
        {
            _featuresData = data;
            _entityLogic = targetCharacter;
            _featuresList = new List<IEntityFeature>();
        }

        public async UniTask PreloadFeaturesAsync()
        {
            foreach (var featureAsset in _featuresData.FeatureAssetList)
            {
                AsyncOperationHandle<GameObject> handle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<GameObject>(featureAsset);
                await handle;

                var featureObj = GameObject.Instantiate(handle.Result);
                featureObj.name = handle.Result.name;

                IEntityFeature feature = featureObj.GetComponent<IEntityFeature>();
                Transform connectionPart = GetConnectionPart(feature.EntityConnectionPart);
                featureObj.transform.SetParent(connectionPart, false);

                _loadedFeaturesHandles.Add(handle);
                _featuresList.Add(feature);
            }

            _isFeaturesLoaded = true;
        }

        public void InitializeFeatures()
        {
            foreach (var item in _featuresList)
            {
                item.Initialize(_entityLogic);
            }
        }

        public T? GetFeature<T>() where T : class, IEntityFeature
        {
            return _featuresList.FirstOrDefault() as T;
        }

        public void Dispose()
        {
            foreach (var item in _featuresList)
            {
                item.Dispose();
            }

            _featuresList.ForEach(item => GameObject.Destroy(item.RootObject));

            UnloadFeatures();
            _featuresList.Clear();
        }

        public void UnloadFeatures()
        {
            if (!_isFeaturesLoaded)
            {
                return;
            }

            foreach (var featureHandle in _loadedFeaturesHandles)
            {
                AddressableAssetsLoader.Instance.UnloadAsset(featureHandle);
            }

            _loadedFeaturesHandles.Clear();
            _isFeaturesLoaded = false;
        }

#nullable restore
        //code with bad nulls of gameobjects:

        private Transform GetConnectionPart(IEntityFeature.TargetEntityPart targetPart)
        {
            return targetPart switch
            {
                IEntityFeature.TargetEntityPart.Base => (_entityLogic as MonoBehaviour).transform,
                IEntityFeature.TargetEntityPart.Body => (_entityLogic.Body.Transform),
                _ => throw new NotImplementedException()
            };
        }

        #endregion
    }
}
