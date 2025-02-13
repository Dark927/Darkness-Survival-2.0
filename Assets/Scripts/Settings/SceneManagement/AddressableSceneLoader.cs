using System.Collections.Generic;
using System.Threading;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Settings.SceneManagement
{
    public class AddressableSceneLoader
    {
        #region Fields 

        private List<SceneInstance> _loadedAdditiveScenes;
        private List<SceneInstance> _scenesToUnload;
        private bool _needClean;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Properties

        public int LoadedScenesCount => _loadedAdditiveScenes.Count;

        #endregion


        #region Methods

        #region Init

        public AddressableSceneLoader()
        {
            _loadedAdditiveScenes = new List<SceneInstance>();
            _needClean = false;
        }

        #endregion


        public void LoadAdditiveSceneClean(AssetReference sceneReference)
        {
            UnloadAll();
            LoadAdditiveScene(sceneReference);
        }

        /// <summary>
        /// Loads additive addressable scene using Asset Reference without clearing previous ones..
        /// </summary>
        /// <param name="sceneReference">Scene to load</param>
        public void LoadAdditiveScene(AssetReference sceneReference)
        {
            if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }

            Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive).Completed += (operationHandler) =>
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _loadedAdditiveScenes.Add(operationHandler.Result);
                    _needClean = true;
                }
                else
                {
                    TryUnloadAdditiveScene(operationHandler.Result);
                }
            };
        }


        /// <summary>
        /// Unload all loaded addressable scenes.
        /// </summary>
        public void UnloadAll()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            if (!_needClean)
            {
                return;
            }

            _scenesToUnload = new List<SceneInstance>(_loadedAdditiveScenes);
            _loadedAdditiveScenes.Clear();

            foreach (var sceneInstance in _scenesToUnload)
            {
                TryUnloadAdditiveScene(sceneInstance);
            }
        }

        private void TryUnloadAdditiveScene(SceneInstance sceneInstance)
        {
            if (!_needClean || !sceneInstance.Scene.isLoaded)
            {
                return;
            }

            Addressables.UnloadSceneAsync(sceneInstance).Completed += (operationHandler) =>
            {
                if (_scenesToUnload.Count == 0)
                {
                    _needClean = false;
                }
            };
        }


        #endregion
    }
}