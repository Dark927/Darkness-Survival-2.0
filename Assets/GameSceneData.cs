using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// This class is a base class which contains what is common to all game scenes
/// </summary>

namespace Settings.SceneManagement
{
    [CreateAssetMenu(fileName = "NewGameSceneData", menuName = "Game/Scenes/Game Scene Data")]
    public class GameSceneData : DescriptionBaseData
    {
        public enum GameSceneType
        {
            //Playable scenes
            Stage,
            Menu,

            //Special scenes
            GlobalManagers,
            Gameplay,
        }

        [SerializeField] private GameSceneType _sceneType;
        [SerializeField] private AssetReference _sceneReference;

        public GameSceneType SceneType => _sceneType;
        public AssetReference SceneReference => _sceneReference;
    }
}