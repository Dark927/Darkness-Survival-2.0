using Settings.AssetsManagement;
using Settings.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Utilities.ErrorHandling;

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

        GameSceneManager.Instance.AddressableLoader.LoadScene(_globalSceneData.SceneReference, LoadSceneMode.Single);
        GameSceneManager.Instance.LoadMainMenu();

        //Initialization is the only scene in BuildSettings, so it has index 0
        SceneManager.UnloadSceneAsync(0);
    }

    #endregion
}
