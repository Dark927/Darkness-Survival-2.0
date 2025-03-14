using Settings.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Utilities.ErrorHandling;

namespace Settings.Global
{
    public class GameInitializer : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private GameSceneData _globalSceneData;

        #endregion


        #region Methods

        private void Awake()
        {
            if (_globalSceneData == null)
            {
                ErrorLogger.LogComponentIsNull(LogOutputType.Console, gameObject.name, $"{nameof(AssetReference)} - start scene data");
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
                return;
            }
        }

        private void Start()
        {
            GameSceneLoadHandler.Instance.RequestSceneLoad(_globalSceneData, LoadSceneMode.Single);
            GameSceneLoadHandler.Instance.RequestMainMenuLoad();
        }

        #endregion
    }
}
