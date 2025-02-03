using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Settings.SceneManagement
{
    public class DefaultSceneLoader
    {
        #region Fields 

        private List<string> _loadedAdditiveScenes;
        private List<string> _markedToUnloadScenes;

        #endregion


        #region Methods

        #region Init

        public DefaultSceneLoader()
        {
            _loadedAdditiveScenes = new List<string>();
        }

        #endregion
        public void LoadSingleScene(string sceneName)
        {
            UnloadAllAdditiveScenes();

            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void LoadAdditiveScene(string sceneName, bool loadAsync = true)
        {
            if (loadAsync)
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (operationHandler) =>
                {
                    _loadedAdditiveScenes.Add(sceneName);
                };
            }
            else
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                _loadedAdditiveScenes.Add(sceneName);
            }
        }


        public void RestartActiveScene()
        {
            UnloadAllAdditiveScenes();
            LoadSingleScene(SceneManager.GetActiveScene().name);
        }

        public void UnloadAllAdditiveScenes()
        {
            if ((_loadedAdditiveScenes == null) || (_loadedAdditiveScenes.Count == 0))
            {
                return;
            }

            _markedToUnloadScenes = new List<string>(_loadedAdditiveScenes);
            _loadedAdditiveScenes.Clear();

            foreach (var sceneName in _markedToUnloadScenes)
            {
                UnloadScene(sceneName);
            }
        }

        public void LoadScenes(string mainScene, params string[] additiveScenes)
        {
            try
            {
                LoadSingleScene(mainScene);

                foreach (string sceneName in additiveScenes)
                {
                    LoadAdditiveScene(sceneName);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void UnloadScene(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        #endregion
    }
}
