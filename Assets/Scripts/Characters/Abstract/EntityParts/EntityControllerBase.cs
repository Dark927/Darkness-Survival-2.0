using System;
using Characters.Common.Features;
using Characters.Common.Settings;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Common
{
    public abstract class EntityControllerBase : MonoBehaviour, IEntityController
    {
        public event Action<IEntityDynamicLogic, IAttackable> OnEntityKilled;

        #region Fields 

        private IEntityDynamicLogic _entityLogic;
        private bool _started = false;

        // Features

        [Header("Features Settings")]
        [SerializeField] private AssetReferenceSO _featuresSetDataRef;

        private EntityCustomFeaturesHolder<IEntityFeature, IFeatureData> _featuresHolder;
        private AsyncOperationHandle<FeatureSetData> _asyncOperationHandle;

        #endregion


        #region Properties 

        public EntityCustomFeaturesHolder<IEntityFeature, IFeatureData> FeaturesHolder => _featuresHolder;
        public IEntityDynamicLogic EntityLogic => _entityLogic;

        #endregion


        #region Methods

        #region Init

        public virtual void Initialize(IEntityData data)
        {
            _entityLogic = GetComponentInChildren<IEntityDynamicLogic>();
        }

        protected async UniTask InitFeaturesAsync()
        {
            if (!AddressableAssetsHandler.IsAssetRefValid(_featuresSetDataRef))
            {
                return;
            }

            _asyncOperationHandle = AddressableAssetsHandler.Instance.LoadAssetAsync<FeatureSetData>(_featuresSetDataRef);
            await _asyncOperationHandle;

            _featuresHolder = new(_entityLogic);
            _featuresHolder.GiveMultipleFeaturesAsync(_asyncOperationHandle.Result.FeatureAssetList).Forget();
        }


        protected virtual void Start()
        {
            _started = true;
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                ConfigureEventLinks();
            }
        }

        public virtual void ConfigureEventLinks()
        {
            _entityLogic.Body.OnKilled += RaiseEntityWasKilled;
        }

        public virtual void RemoveEventLinks()
        {
            _entityLogic.Body.OnKilled -= RaiseEntityWasKilled;
        }

        protected virtual void OnDisable()
        {
            RemoveEventLinks();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            RemoveFeatures();
        }

        #endregion

        public void RemoveFeatures()
        {
            if (FeaturesHolder != null)
            {
                FeaturesHolder.Dispose();
                AddressableAssetsHandler.Instance.UnloadAsset(_asyncOperationHandle);
                _featuresHolder = null;
            }
        }

        private void RaiseEntityWasKilled(IAttackable killer)
        {
            OnEntityKilled?.Invoke(this.EntityLogic, killer);
        }

        #endregion
    }
}
