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

namespace Settings.SceneManagement
{

    /// <summary>
    /// This class is a main class to load scenes using default/addressable scene loaders.
    /// </summary>
    public class GameSceneManager : LazySingletonMono<GameSceneManager>
    {
        #region Fields

        [SerializeField] private AssetReference _loadingScreenRef;
        [SerializeField] private GameSceneData _mainMenuScene;
        [SerializeField] private GameSceneData _gameplayEssentialsScene;
        [SerializeField] private List<GameSceneData> _additiveScenesData;

        private DefaultSceneLoader _defaultSceneLoader = new DefaultSceneLoader();
        private AddressableSceneLoader _addressableSceneLoader = new AddressableSceneLoader();

        private Dictionary<Type, (UnityEngine.Object createdObj, AsyncOperationHandle loadHandle)> _loadedObjects;

        #endregion


        #region Properties 

        public DefaultSceneLoader DefaultLoader => _defaultSceneLoader;
        public AddressableSceneLoader AddressableLoader => _addressableSceneLoader;

        #endregion


        #region Methods

        #region Init

        protected override void AwakeInit()
        {
            base.AwakeInit();
            DontDestroyOnLoad(this);
            _loadedObjects = new Dictionary<Type, (UnityEngine.Object loadedComponent, AsyncOperationHandle loadHandle)>();
        }

        #endregion

        public void AddAdditiveSceneToLoad(GameSceneData sceneData)
        {
            _additiveScenesData.Add(sceneData);
        }

        public void LoadMainMenu()
        {
            if (_mainMenuScene == null)
            {
                Debug.LogWarning($" # Can not load the main menu scene, {_mainMenuScene} is null!");
            }

            AddressableLoader.LoadAdditiveSceneClean(_mainMenuScene.SceneReference);
        }

        public void StartStage()
        {
            StartStageAsync().Forget();
        }

        public void UnloadAll()
        {
            AddressableLoader.UnloadAll();
            DefaultLoader.UnloadAllAdditiveScenes();
        }

        public void CleanLoad()
        {
            foreach (var sceneData in _additiveScenesData)
            {
                AddressableLoader.LoadAdditiveSceneClean(sceneData.SceneReference);
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

        private async UniTaskVoid StartStageAsync()
        {
            LoadingScreenUI loadingScreen = await CreateLoadingScreen(_loadingScreenRef);

            DefaultLoader.UnloadAllAdditiveScenes();
            AddressableLoader.LoadAdditiveSceneClean(_gameplayEssentialsScene.SceneReference);

            int progressDivider = _additiveScenesData.Count;
            float totalProgress = 0f;

            List<AsyncOperationHandle<SceneInstance>> sceneLoadHandles = new List<AsyncOperationHandle<SceneInstance>>();

            // Start loading all scenes
            foreach (var sceneData in _additiveScenesData)
            {
                AsyncOperationHandle<SceneInstance> sceneLoadHandle = AddressableLoader.LoadScene(sceneData.SceneReference, LoadSceneMode.Additive, false);
                sceneLoadHandles.Add(sceneLoadHandle);
            }

            // Continuously update loading progress while all scenes are loading
            bool allScenesLoaded = false;

            while (!allScenesLoaded)
            {
                totalProgress = 0f;

                // Calculate total progress based on all scenes
                foreach (var handle in sceneLoadHandles)
                {
                    totalProgress += handle.PercentComplete;
                }

                // Divide by the number of scenes to get the average progress
                totalProgress /= progressDivider;

                // Update the loading screen with the total progress
                loadingScreen.SetLoadingProgress(totalProgress);

                // Check if all scenes have finished loading
                allScenesLoaded = sceneLoadHandles.All(handle => handle.IsDone);

                // Wait for the next frame before checking again
                await UniTask.Yield();
            }

            await UniTask.WaitUntil(() => loadingScreen.IsFullProgress);
            await loadingScreen.Deactivate();

            foreach (var handle in sceneLoadHandles)
            {
                await handle.Result.ActivateAsync();
            }

            RemoveLoadingScreen();
        }

        private async UniTask<LoadingScreenUI> CreateLoadingScreen(AssetReference loadingScreenRef)
        {
            AsyncOperationHandle<GameObject> loadingScreenHandle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<GameObject>(loadingScreenRef);
            await loadingScreenHandle.Task;

            GameObject loadingScreenObj = GameObject.Instantiate(loadingScreenHandle.Result);
            loadingScreenObj.name = nameof(LoadingScreenUI);
            DontDestroyOnLoad(loadingScreenObj);

            _loadedObjects.Add(typeof(LoadingScreenUI), (loadingScreenObj, loadingScreenHandle));

            return loadingScreenObj.GetComponent<LoadingScreenUI>();
        }

        private void RemoveLoadingScreen()
        {
            if (_loadedObjects.TryGetValue(typeof(LoadingScreenUI), out (UnityEngine.Object obj, AsyncOperationHandle handle) loadInfo))
            {
                Destroy((loadInfo.obj as GameObject));
                AddressableAssetsLoader.Instance.UnloadAsset(loadInfo.handle);
                _loadedObjects.Remove(typeof(LoadingScreenUI));
            }
        }

        #endregion
    }
}
