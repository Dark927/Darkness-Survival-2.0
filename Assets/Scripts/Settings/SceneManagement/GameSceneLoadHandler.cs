using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Settings.Abstract;
using Settings.AssetsManagement;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Utilities.ErrorHandling;

namespace Settings.SceneManagement
{

    /// <summary>
    /// This class is a main class to load scenes with cleaning.
    /// </summary>
    public class GameSceneLoadHandler : LazySingletonMono<GameSceneLoadHandler>
    {
        #region Fields

        [SerializeField] private AssetReference _loadingScreenRef;
        [SerializeField] private GameSceneData _mainMenuScene;
        [SerializeField] private GameSceneData _gameplayEssentialsScene;
        [SerializeField] private GameSceneData _gameplayStageScene;

        private IConcreteSceneLoader<AssetReference, AsyncOperationHandle<SceneInstance>> _sceneLoader;
        private CleanEvent _sceneCleanEvent;
        private Dictionary<Type, (UnityEngine.Object createdObj, AsyncOperationHandle loadHandle)> _loadedObjects;

        private GameSceneData _currentStageData;

        #endregion


        #region Properties 

        public IConcreteSceneLoader<AssetReference, AsyncOperationHandle<SceneInstance>> SceneLoader => _sceneLoader;
        public CleanEvent SceneCleanEvent => _sceneCleanEvent;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _sceneLoader = new AddressableSceneLoader();
            _loadedObjects = new Dictionary<Type, (UnityEngine.Object loadedComponent, AsyncOperationHandle loadHandle)>();
            _sceneCleanEvent = new CleanEvent();
        }

        #endregion

        public void RequestMainMenuLoad(bool useLoadingScreen = true)
        {
            LoadSceneAsync(() =>
            {
                SceneCleanEvent?.ListenEvent(this, EventArgs.Empty);
                SceneLoader.UnloadAll();

                return new List<AsyncOperationHandle<SceneInstance>>()
                {
                    SceneLoader.LoadScene(_mainMenuScene.SceneReference, LoadSceneMode.Additive)
                };
            }, useLoadingScreen).Forget();
        }

        public void RequestStageLoad(GameSceneData stageData = null, Action callback = default, bool useLoadingScreen = true)//
        {
            // -----------------------------------------------------------------------------
            // ToDo : this logic will be replaced with the level selection in the future
            // Just use the default stage for tests and MVP.
            // -----------------------------------------------------------------------------
            useLoadingScreen = true;

            if (stageData == null)
            {
                stageData = _gameplayStageScene;
            }

            // -----------------------------------------------------------------------------

            LoadSceneAsync(() =>
            {
                // Cleaning logic
                SceneCleanEvent?.ListenEvent(this, EventArgs.Empty);
                SceneLoader.UnloadAll();

                // Loading logic 
                List<AsyncOperationHandle<SceneInstance>> loadHandles = new()
                {
                    SceneLoader.LoadScene(_gameplayEssentialsScene.SceneReference, LoadSceneMode.Additive),
                    SceneLoader.LoadScene(stageData.SceneReference, LoadSceneMode.Additive)
                };
                _currentStageData = stageData;

                return loadHandles;

            }, useLoadingScreen)
                .ContinueWith(() => callback?.Invoke())
                .Forget();
        }

        public void ReloadCurrentStage(Action callback = default, bool useLoadingScreen = true)
        {
            if (_currentStageData == null)
            {
                ErrorLogger.LogComponentIsNull(gameObject.name, $"the field {nameof(_currentStageData)} of type {nameof(GameSceneData)}");
                return;
            }

            RequestStageLoad(_currentStageData, callback, useLoadingScreen);
        }

        public AsyncOperationHandle<SceneInstance> RequestSceneLoad(GameSceneData sceneData, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            return SceneLoader.LoadScene(sceneData.SceneReference, loadMode);
        }


        private async UniTask LoadSceneAsync(Func<IEnumerable<AsyncOperationHandle<SceneInstance>>> sceneLoadingLogic, bool useLoadingScreen = true)
        {
            if (sceneLoadingLogic == null)
            {
                return;
            }

            LoadingScreenUI loadingScreen = null;

            if (useLoadingScreen)
            {
                loadingScreen = await CreateLoadingScreen(_loadingScreenRef);
            }

            var loadHandles = sceneLoadingLogic.Invoke();

            if (loadingScreen == null)
            {
                return;
            }

            if (loadHandles.Count() > 1)
            {
                await ExecuteLoadingScreen(loadingScreen, loadHandles);
            }
            else
            {
                await ExecuteLoadingScreen(loadingScreen, loadHandles.First());
            }

        }

        private void OnValidate()
        {
            if (_mainMenuScene != null && (_mainMenuScene.SceneType != GameSceneData.GameSceneType.Menu))
            {
                _mainMenuScene = null;
                Debug.LogWarning($"{nameof(_mainMenuScene)} {nameof(GameSceneData.GameSceneType)} is not Menu!");
            }
        }


        #region Loading Screen

        private async UniTask ExecuteLoadingScreen(LoadingScreenUI loadingScreen, IEnumerable<AsyncOperationHandle<SceneInstance>> sceneLoadHandles)
        {
            int progressDivider = sceneLoadHandles.Count();

            if (progressDivider == 0)
            {
                return;
            }

            float totalProgress = 0f;
            bool allScenesLoaded = false;

            while (!allScenesLoaded)
            {
                allScenesLoaded = true;
                totalProgress = 0f;

                // Calculate total progress based on all scenes
                foreach (var handle in sceneLoadHandles)
                {
                    totalProgress += handle.PercentComplete;
                }

                // Divide by the number of scenes to get the average progress
                totalProgress /= progressDivider;

                loadingScreen.SetLoadingProgress(totalProgress);

                //Check if all scenes have finished loading
                allScenesLoaded = sceneLoadHandles.All(handle => handle.IsDone);

                // Wait for the next frame before checking again
                await UniTask.Yield();
            }

            loadingScreen.SetFullProgress();
            await UniTask.WaitUntil(() => loadingScreen.IsFullProgress);

            await loadingScreen.Deactivate();
            RemoveLoadingScreen();
        }

        private async UniTask ExecuteLoadingScreen(LoadingScreenUI loadingScreen, AsyncOperationHandle<SceneInstance> sceneLoadHandle)
        {
            while (!sceneLoadHandle.IsDone)
            {
                loadingScreen.SetLoadingProgress(sceneLoadHandle.PercentComplete);
                await UniTask.Yield();
            }

            loadingScreen.SetFullProgress();
            await UniTask.WaitUntil(() => loadingScreen.IsFullProgress);

            await loadingScreen.Deactivate();
            RemoveLoadingScreen();
        }


        private async UniTask<LoadingScreenUI> CreateLoadingScreen(AssetReference loadingScreenRef)
        {
            AsyncOperationHandle<GameObject> loadingScreenHandle = AddressableAssetsHandler.Instance.TryLoadAssetAsync<GameObject>(loadingScreenRef);
            await loadingScreenHandle.Task;

            GameObject loadingScreenObj = GameObject.Instantiate(loadingScreenHandle.Result);
            loadingScreenObj.name = nameof(LoadingScreenUI);
            GameObject.DontDestroyOnLoad(loadingScreenObj);

            _loadedObjects.Add(typeof(LoadingScreenUI), (loadingScreenObj, loadingScreenHandle));

            return loadingScreenObj.GetComponent<LoadingScreenUI>();
        }


        private void RemoveLoadingScreen()
        {
            if (_loadedObjects.TryGetValue(typeof(LoadingScreenUI), out (UnityEngine.Object obj, AsyncOperationHandle handle) loadInfo))
            {
                GameObject.Destroy((loadInfo.obj as GameObject));
                AddressableAssetsHandler.Instance.UnloadAsset(loadInfo.handle);
                _loadedObjects.Remove(typeof(LoadingScreenUI));
            }
        }

        #endregion


        #endregion

    }
}
