using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using UnityEngine;

#nullable enable

namespace Characters.Common.Features
{
    public class EntityCustomFeaturesHolder : EntityFeaturesHolderBase<IEntityFeature>
    {
        #region Methods

        #region Init
        public EntityCustomFeaturesHolder(IEntityDynamicLogic targetEntity) : base(targetEntity)
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

            var handle = AddressableAssetsHandler.Instance.LoadAssetAndCacheAsync<GameObject>(featureData.AssetRef, AddressableAssetsCleaner.CleanType.SceneSwitch, false);
            await handle;

            IEntityFeature loadedFeature = handle.Result.GetComponent<IEntityFeature>();
            IEntityFeature.TargetEntityPart? targetPart = loadedFeature?.EntityConnectionPart;

            Transform? connectionPart = GetConnectionPart(targetPart);

            if (connectionPart != null)
            {
                string featureName = string.IsNullOrEmpty(featureData.Name) ? handle.Result.name : featureData.Name;
                var createdFeature = CreateFeature(handle.Result, featureName, connectionPart);
                createdFeature.Initialize(EntityLogic);

                ActiveOnesDict.TryAdd(featureData.ID, createdFeature);
            }
        }

        public T? GetFeature<T>(int ID) where T : class, IEntityFeature
        {
            return ActiveOnesDict[ID] as T;
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
