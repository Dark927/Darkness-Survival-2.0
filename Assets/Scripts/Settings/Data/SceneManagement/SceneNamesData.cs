﻿
using UnityEngine;

namespace Settings.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneNames", menuName = "Game/Scenes/DELETE ME")]
    public class SceneNamesData : ScriptableObject
    {
        [SerializeField] private string _globalScene;
        [SerializeField] private string _gameplayEssentialsScene;

        public string GlobalScene => _globalScene;
        public string GameplayEssentialsScene => _gameplayEssentialsScene;
    }
}
