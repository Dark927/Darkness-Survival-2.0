using Settings.Abstract;
using Settings.AssetsManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI
{
    public class GamePanelManager : LazySingletonMono<GamePanelManager>
    {
        public enum PanelType
        {
            Settings = 1,
            Credits = 2,
        }

        [SerializeField] private AssetReference _gameSettingsMenuRef;
        [SerializeField] private AssetReference _creditsMenuRef;

        private Queue<(GameObject panelObj, AsyncOperationHandle handle)> _activePanelsQueue;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _activePanelsQueue = new Queue<(GameObject panelObj, AsyncOperationHandle handle)>();
        }

        public void OpenPanel(PanelType panelType, Canvas parentCanvas = null)
        {
            switch (panelType)
            {
                case PanelType.Settings:
                    OpenSettingsPanel(parentCanvas);
                    break;

                case PanelType.Credits:
                    OpenCreditsPanel(parentCanvas);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void CloseCurrentPanel()
        {
            if (_activePanelsQueue.Count == 0)
            {
                return;
            }

            var currentPanelInfo = _activePanelsQueue.Dequeue();
            Destroy(currentPanelInfo.panelObj);
            AddressableAssetsLoader.Instance.UnloadAsset(currentPanelInfo.handle);
        }

        private void OpenSettingsPanel(Canvas parentCanvas = null)
        {
            OpenPanel(_gameSettingsMenuRef);
        }

        private void OpenCreditsPanel(Canvas parentCanvas)
        {
            OpenPanel(_creditsMenuRef, (obj) =>
            {
                if (parentCanvas != null)
                {
                    obj.transform.SetParent(parentCanvas.transform, false);
                }
            });
        }

        private void OpenPanel(AssetReference reference, Action<GameObject> panelConfiguration = null)
        {
            AsyncOperationHandle<GameObject> loadHandle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<GameObject>(reference);

            loadHandle.Completed += (handle) =>
            {
                GameObject menuObj = Instantiate(handle.Result);
                panelConfiguration?.Invoke(menuObj);

                _activePanelsQueue.Enqueue((menuObj, handle));
            };
        }

    }
}
