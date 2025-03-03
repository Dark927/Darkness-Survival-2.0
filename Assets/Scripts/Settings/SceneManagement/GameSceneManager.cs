using Settings.Abstract;
using Settings.Global;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Settings.SceneManagement
{

    /// <summary>
    /// This class is a main class to load scenes using default/addressable scene loaders.
    /// </summary>
    public class GameSceneManager : LazySingletonMono<GameSceneManager>
    {
        #region Fields

        [SerializeField] private GameSceneData _mainMenuScene;
        [SerializeField] private GameSceneData _gameplayEssentialsScene;
        [SerializeField] private List<GameSceneData> _additiveScenesData;

        private DefaultSceneLoader _defaultSceneLoader = new DefaultSceneLoader();
        private AddressableSceneLoader _addressableSceneLoader = new AddressableSceneLoader();

        #endregion


        #region Properties 

        public  DefaultSceneLoader DefaultLoader => _defaultSceneLoader;
        public AddressableSceneLoader AddressableLoader => _addressableSceneLoader;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            DontDestroyOnLoad(this);
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
            DefaultLoader.UnloadAllAdditiveScenes();

            AddressableLoader.LoadAdditiveSceneClean(_gameplayEssentialsScene.SceneReference);

            foreach (var sceneData in _additiveScenesData)
            {
                AddressableLoader.LoadScene(sceneData.SceneReference, LoadSceneMode.Additive);
            }
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
            if(_mainMenuScene != null && (_mainMenuScene.SceneType != GameSceneData.GameSceneType.Menu))
            {
                _mainMenuScene = null;
                Debug.LogWarning($"{nameof(_mainMenuScene)} {nameof(GameSceneData.GameSceneType)} is not Menu!");
            }
        }

        #endregion
    }
}
