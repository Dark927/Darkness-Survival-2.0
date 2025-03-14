using System;
using System.Collections.Generic;
using Gameplay.Stage;
using Settings.AssetsManagement;
using Settings.Global;
using Settings.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace UI
{
    public class GamePanelManagerUI : MonoBehaviour, IEventListener
    {
        public enum PanelType
        {
            Default = 0,
            Settings,
            Credits,
            Pause,
            GameOver,
            GameWin,
        }

        #region Fields 

        public const string DefaultParentCanvasName = "Panels-Canvas";

        private Canvas _parentCanvas;

        private GamePanelDataUI _gamePanelData;

        private Dictionary<int, (GameObject panelObj, AsyncOperationHandle handle)> _activePanels;
        private bool _isPanelProcessing = false;

        private DiContainer _container;

        #endregion


        #region Methods 

        #region Init 

        public void Initialize(DiContainer container, Canvas targetCanvas, GamePanelDataUI data)
        {
            DontDestroyOnLoad(gameObject);
            _parentCanvas = targetCanvas;
            _container = container;
            _gamePanelData = data;
            _activePanels = new();

            GameSceneLoadHandler.Instance.SceneCleanEvent.Subscribe(this);
        }

        public void OnDestroy()
        {
            GameSceneLoadHandler.Instance.SceneCleanEvent.Unsubscribe(this);
        }

        #endregion 

        public bool TryOpenPanel(PanelType panelType, Action callback = default)
        {
            if (_isPanelProcessing)
            {
                return false;
            }

            _isPanelProcessing = true;

            AsyncOperationHandle loadPanelHandle;

            loadPanelHandle = panelType switch
            {
                PanelType.Settings => OpenPanel(_gamePanelData.GameSettingsMenuRef, parent: _parentCanvas.transform),
                PanelType.Credits => OpenPanel(_gamePanelData.CreditsMenuRef, parent: _parentCanvas.transform),
                PanelType.Pause => OpenPanel(_gamePanelData.PauseMenuRef, parent: _parentCanvas.transform),
                PanelType.GameOver => OpenPanelWithStats(_gamePanelData.GameOverMenuRef, _parentCanvas.transform),
                PanelType.GameWin => OpenPanelWithStats(_gamePanelData.GameWinMenuRef, _parentCanvas.transform),

                PanelType.Default => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };

            loadPanelHandle.Task.ContinueWith((handle) =>
            {
                callback?.Invoke();
            });

            return true;
        }

        private AsyncOperationHandle OpenPanelWithStats(AssetReference panelReference, Transform parent = null)
        {
            return OpenPanel(panelReference, (panelObj) =>
                    {
                        StageProgressService progressService = ServiceLocator.Current.Get<StageProgressService>();

                        if (progressService != null)
                        {
                            GameplayInfoMenuUI gameOverMenu = panelObj.GetComponentInChildren<GameplayInfoMenuUI>();
                            StageProgress progress = progressService.CollectStageProgress(true);
                            gameOverMenu.SetProgressStats(progress);
                        }

                    }, parent: parent);
        }

        public void ClosePauseMenu(Action callback = default)
        {
            if (_activePanels.Count == 0)
            {
                return;
            }

            foreach (var panelInfo in _activePanels.Values)
            {
                if (panelInfo.panelObj.TryGetComponent(out PauseMenuUI pauseMenu))
                {
                    pauseMenu.RequestDeactivation(callback);
                    return;
                }
            }
        }

        public void CloseAllPanels(Action callback = default)
        {
            if (_activePanels == null)
            {
                callback?.Invoke();
                return;
            }

            IMenuUI panel;

            foreach (var panelInfo in _activePanels.Values)
            {
                panel = panelInfo.panelObj.GetComponent<IMenuUI>();
                panel.RequestDeactivation();
            }

            callback?.Invoke();
        }

        /// <summary>
        /// Check if the panel can be closed and gives an out delegate with post close actions.
        /// </summary>
        /// <param name="panel">the target panel</param>
        /// <param name="postCloseCallback">actions after closing the panel (if the panel can be closed, else - null)</param>
        /// <returns>true if panel can be closed, else - false</returns>
        public bool CanClosePanel(IMenuUI panel, out Action postCloseCallback)
        {
            postCloseCallback = null;
            bool canClosePanel = !_isPanelProcessing && (panel != null);
            canClosePanel &= (_activePanels != null) && _activePanels.Count != 0;

            if (!canClosePanel)
            {
                return canClosePanel;
            }

            GameObject panelObj = ((MonoBehaviour)panel).gameObject;
            int panelID = panelObj.GetInstanceID();

            canClosePanel &= _activePanels.TryGetValue(panelID, out var targetPanelInfo);

            if (canClosePanel)
            {
                _isPanelProcessing = true;
                postCloseCallback = () =>
                {
                    GameObject.Destroy(targetPanelInfo.panelObj);
                    AddressableAssetsHandler.Instance.UnloadAsset(targetPanelInfo.handle);
                    _activePanels.Remove(panelID);
                    _isPanelProcessing = false;
                };
            }

            return canClosePanel;
        }

        public void Listen(object sender, EventArgs args)
        {
            if (sender is GameSceneLoadHandler)
            {
                CloseAllPanels();
            }
        }

        private AsyncOperationHandle OpenPanel(AssetReference reference, Action<GameObject> panelConfiguration = null, Transform parent = null)
        {
            AsyncOperationHandle<GameObject> loadHandle = AddressableAssetsHandler.Instance.TryLoadAssetAsync<GameObject>(reference);

            loadHandle.Completed += (handle) =>
            {
                GameObject menuObj = Instantiate(handle.Result);
                _container.Inject(menuObj.GetComponent<IMenuUI>());

                menuObj.name = handle.Result.name;

                if (parent != null)
                {
                    menuObj.transform.SetParent(parent, false);
                    menuObj.transform.SetAsFirstSibling();
                }

                panelConfiguration?.Invoke(menuObj);
                _activePanels.Add(menuObj.GetInstanceID(), (menuObj, handle));

                TryDisplayPanel(menuObj);
            };

            return loadHandle;
        }


        /// <summary>
        /// Check if the panel obj has animations and show them
        /// </summary>
        /// <param name="menuObj">target panel</param>
        private void TryDisplayPanel(GameObject menuObj)
        {
            BasicMenuUI menu = menuObj.GetComponentInChildren<BasicMenuUI>();

            if (menu != null)
            {
                menu.Activate(() =>
                {
                    _isPanelProcessing = false;
                });
            }
        }

        #endregion
    }
}
