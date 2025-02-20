using Characters.Common.Features;
using Characters.Interfaces;
using Characters.Stats;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using Settings.Global;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Common
{
    public abstract class EntityControllerBase : MonoBehaviour, IEventListener, IDisposable
    {
        #region Fields 

        private IEntityDynamicLogic _entityLogic;
        private bool _started = false;

        // Features

        [Header("Features Settings")]
        [SerializeField] private AssetReferenceSO _featuresSetDataRef;
        private EntityCustomFeaturesHolder _featuresHolder;
        private AsyncOperationHandle<FeatureSetData> _asyncOperationHandle;

        #endregion


        #region Properties 

        public EntityCustomFeaturesHolder FeaturesHolder => _featuresHolder;
        public IEntityDynamicLogic EntityLogic => _entityLogic;

        #endregion


        #region Methods

        #region Init

        public virtual void Initialize(IEntityData data)
        {
            _entityLogic = GetComponentInChildren<IEntityDynamicLogic>();
        }

        public async UniTask InitFeaturesAsync()
        {
            if (!AddressableAssetsLoader.IsAssetRefValid(_featuresSetDataRef))
            {
                return;
            }

            _asyncOperationHandle = AddressableAssetsLoader.Instance.LoadAssetAsync<FeatureSetData>(_featuresSetDataRef);
            await _asyncOperationHandle;

            _featuresHolder = new EntityCustomFeaturesHolder(_entityLogic);
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

        }

        public virtual void RemoveEventLinks()
        {

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
                AddressableAssetsLoader.Instance.UnloadAsset(_asyncOperationHandle);
                _featuresHolder = null;
            }
        }

        #endregion
    }
}
