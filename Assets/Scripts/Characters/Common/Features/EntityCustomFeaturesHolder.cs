using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable enable

namespace Characters.Common.Features
{
    public class EntityCustomFeaturesHolder : EntityFeaturesHolderBase<IEntityFeature>
    {
        #region Properties

        public IEnumerable<IEntityFeature> ActiveFeatures => LoadedFeaturesDict.Keys;


        #endregion


        #region Methods

        #region Init
        public EntityCustomFeaturesHolder(IEntityLogic targetEntity) : base(targetEntity)
        {
        }

        #endregion

        public async override UniTask GiveFeatureAsync<TFeatureData>(TFeatureData data)
        {
            FeatureData? featureData = data as FeatureData;

            if (featureData == null)
            {
                return;
            }

            AsyncOperationHandle<GameObject> handle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<GameObject>(featureData.AssetRef);
            await handle;

            IEntityFeature loadedFeature = handle.Result.GetComponent<IEntityFeature>();
            IEntityFeature.TargetEntityPart? targetPart = loadedFeature?.EntityConnectionPart;

            Transform? connectionPart = GetConnectionPart(targetPart);

            if (connectionPart != null)
            {
                string featureName = string.IsNullOrEmpty(featureData.Name) ? handle.Result.name : featureData.Name;
                var createdFeature = CreateFeature(handle.Result, featureName, connectionPart);
                createdFeature.Initialize(EntityLogic);
                LoadedFeaturesDict.Add(createdFeature, handle);
            }
        }

        public T? GetFeature<T>() where T : class, IEntityFeature
        {
            return ActiveFeatures.FirstOrDefault() as T;
        }

        protected override void DestroyFeatureLogic(IEntityFeature feature)
        {
            GameObject.Destroy(feature.RootObject);
        }

        private Transform? GetConnectionPart(IEntityFeature.TargetEntityPart? targetPart)
        {
            return targetPart switch
            {
                IEntityFeature.TargetEntityPart.Base => EntityLogic is MonoBehaviour monoBehaviour ? monoBehaviour.transform : null,
                IEntityFeature.TargetEntityPart.Body => EntityLogic.Body?.Transform,
                _ => null,
            };
        }

        #endregion
    }
}
