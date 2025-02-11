using Settings.Abstract;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Settings.SceneManagement
{
    public class GameSceneManager : SingletonBase<GameSceneManager>
    {
        #region Fields

        [SerializeField] private List<AssetReference> _stageScenesReferences;

        [SerializeField] private SceneNames _mainSceneNames;

        private DefaultSceneLoader _defaultSceneLoader;
        private AddressableSceneLoader _addressableSceneLoader;

        #endregion


        #region Properties 

        public DefaultSceneLoader DefaultSceneLoader => _defaultSceneLoader;
        public AddressableSceneLoader AddressableSceneLoader => _addressableSceneLoader;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            InitComponents();
            InitInstance(true);

            if (_mainSceneNames == null)
            {
                Debug.LogWarning($"{nameof(_mainSceneNames)} is null!");
            }
        }

        private void InitComponents()
        {
            _defaultSceneLoader = new DefaultSceneLoader();
            _addressableSceneLoader = new AddressableSceneLoader();
        }

        #endregion

        public void RestartStage()
        {
            AddressableSceneLoader.UnloadAll();
            DefaultSceneLoader.UnloadAllAdditiveScenes();

            DefaultSceneLoader.LoadSingleScene(_mainSceneNames.GlobalScene);
            DefaultSceneLoader.LoadAdditiveScene(_mainSceneNames.GameplayEssentialsScene, false);

            foreach (var scene in _stageScenesReferences)
            {
                AddressableSceneLoader.LoadAdditiveScene(scene);
            }
        }

        public void UnloadAll()
        {
            AddressableSceneLoader.UnloadAll();
            DefaultSceneLoader.UnloadAllAdditiveScenes();
        }

        public void CleanLoad()
        {
            foreach (var scene in _stageScenesReferences)
            {
                AddressableSceneLoader.LoadAdditiveSceneClean(scene);
            }
        }

        #endregion
    }
}
