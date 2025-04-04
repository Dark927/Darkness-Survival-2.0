
using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities.Json;

namespace Gameplay.Stage
{
    public static class GameSavePaths
    {
        private const string DefaultFileName = "DefaultGameConfig.json";
        private const string ConfigFileName = "GameSavePaths.json"; // JSON config file

        private static string _stageProgressFileName = DefaultFileName;

        // Property to access the dynamically loaded path
        public static string StageProgressFilePath => Path.Combine(Application.persistentDataPath, _stageProgressFileName);

        // Static constructor to load paths on first access
        public static void InitializeAsync()
        {
            // Loads save paths from a JSON file.
            // If the file doesn't exist, it creates one with default values.

            UniTask.Void(async () =>
            {
                string configPath = Path.Combine(Application.persistentDataPath, ConfigFileName);
                var loadedConfig = await JsonHelper.TryLoadFromJsonAsync<GameSavePathsConfig>(configPath);

                if (loadedConfig.success)
                {
                    if (!string.IsNullOrEmpty(loadedConfig.result.StageProgressFilePath))
                    {
                        _stageProgressFileName = loadedConfig.result.StageProgressFilePath;
                    }
                    else
                    {
                        SaveDefaultPaths(configPath);
                    }
                }
                else
                {
                    SaveDefaultPaths(configPath);
                }

                await UniTask.Yield();
            });
        }

        private static void SaveDefaultPaths(string configPath)
        {
            GameSavePathsConfig defaultConfig = new GameSavePathsConfig();
            JsonHelper.SaveToJsonAsync(defaultConfig, configPath).Forget();
        }
    }

    /// <summary>
    /// Data structure for JSON configuration.
    /// </summary>
    [Serializable]
    public struct GameSavePathsConfig
    {
        public string StageProgressFilePath;

        public GameSavePathsConfig(string defaultName = "")
        {
            StageProgressFilePath = "Stage/StageProgress.json";

        }
    }
}

