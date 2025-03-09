using System;
using System.Collections.Generic;
using Settings.Abstract;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace UI
{
    public class GamePanelManagerUI : LazySingletonMono<GamePanelManagerUI>
    {
        public enum PanelType
        {
            Settings = 1,
            Credits = 2,
        }

        #region Fields 

        public const string DefaultParentCanvasName = "Panels-Canvas";

        private Canvas _parentCanvas;

        [SerializeField] private AssetReference _gameSettingsMenuRef;
        [SerializeField] private AssetReference _creditsMenuRef;

        private Queue<(GameObject panelObj, AsyncOperationHandle handle)> _activePanelsQueue;

        #endregion


        #region Methods 

        #region Init 

        [Inject]
        public void Construct(Canvas targetCanvas)
        {
            _parentCanvas = targetCanvas;
        }

        protected override void AwakeInit()
        {
            base.AwakeInit();
            DontDestroyOnLoad(this);
            _activePanelsQueue = new Queue<(GameObject panelObj, AsyncOperationHandle handle)>();
        }

        #endregion

        public void OpenPanel(PanelType panelType)
        {
            switch (panelType)
            {
                case PanelType.Settings:
                    OpenSettingsPanel(_parentCanvas);
                    break;

                case PanelType.Credits:
                    OpenCreditsPanel(_parentCanvas);
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

            TryHideAnimatedPanel(currentPanelInfo.panelObj, () =>
            {
                Destroy(currentPanelInfo.panelObj);
                AddressableAssetsLoader.Instance.UnloadAsset(currentPanelInfo.handle);
            });
        }

        private void OpenSettingsPanel(Canvas parentCanvas = null)
        {
            OpenPanel(_gameSettingsMenuRef, parent: parentCanvas.transform);
        }

        private void OpenCreditsPanel(Canvas parentCanvas)
        {
            OpenPanel(_creditsMenuRef, parent: parentCanvas.transform);

        }

        private void OpenPanel(AssetReference reference, Action<GameObject> panelConfiguration = null, Transform parent = null)
        {
            AsyncOperationHandle<GameObject> loadHandle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<GameObject>(reference);

            loadHandle.Completed += (handle) =>
            {
                GameObject menuObj = Instantiate(handle.Result);
                menuObj.name = handle.Result.name;

                if (parent != null)
                {
                    menuObj.transform.SetParent(parent, false);
                    menuObj.transform.SetAsFirstSibling();
                }

                panelConfiguration?.Invoke(menuObj);
                _activePanelsQueue.Enqueue((menuObj, handle));

                TryDisplayAnimatedPanel(menuObj);
            };
        }


        /// <summary>
        /// Check if the panel obj has animations and show them
        /// </summary>
        /// <param name="menuObj">target panel</param>
        private static void TryDisplayAnimatedPanel(GameObject menuObj)
        {
            DefaultMenuPopupUI animatedMenu = menuObj.GetComponentInChildren<DefaultMenuPopupUI>();

            if (animatedMenu != null)
            {
                animatedMenu.PrepareAnimation();
                animatedMenu.Show();
            }
        }


        /// <summary>
        /// Check if the panel obj has animations and show them before closing 
        /// </summary>
        private static void TryHideAnimatedPanel(GameObject menuObj, Action callback = null)
        {
            DefaultMenuPopupUI animatedMenu = menuObj.GetComponentInChildren<DefaultMenuPopupUI>();

            if (animatedMenu != null)
            {
                animatedMenu.Hide(callback);
                return;
            }

            callback?.Invoke();
        }

        #endregion
    }
}
