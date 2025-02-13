using Characters.Common.Features;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using Settings.Global;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Common
{
    public abstract class EntityController : MonoBehaviour, IEventListener, IDisposable
    {
        #region Fields 

        private IEntityLogic _entityLogic;
        private bool _started = false;

        // Features

        [Header("Features Settings")]
        [SerializeField] private AssetReference _entityFeaturesDataAsset;
        private EntityFeaturesHolder _featuresHolder;
        private AsyncOperationHandle<EntityFeaturesData> _asyncOperationHandle;

        #endregion


        #region Properties 

        public EntityFeaturesHolder FeaturesHolder => _featuresHolder;
        public IEntityLogic EntityLogic => _entityLogic;

        #endregion


        #region Methods

        #region Init

        protected virtual void Awake()
        {
            _entityLogic = GetComponentInChildren<IEntityLogic>();
        }

        public async UniTask InitFeaturesAsync()
        {
            if (!AddressableAssetsLoader.IsAssetRefValid(_entityFeaturesDataAsset))
            {
                return;
            }

            _asyncOperationHandle = AddressableAssetsLoader.Instance.LoadAssetAsync<EntityFeaturesData>(_entityFeaturesDataAsset);
            await _asyncOperationHandle;

            _featuresHolder = new EntityFeaturesHolder(_entityLogic, _asyncOperationHandle.Result);
            await _featuresHolder.PreloadFeaturesAsync();
            _featuresHolder.InitializeFeatures();
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
