
using System.IO;
using UnityEngine;
using System;
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
        static GameSavePaths()
        {
            LoadPathsFromJson();
        }

        /// <summary>
        /// Loads save paths from a JSON file.
        /// If the file doesn't exist, it creates one with default values.
        /// </summary>
        private static void LoadPathsFromJson()
        {
            string configPath = Path.Combine(Application.persistentDataPath, ConfigFileName);

            if (JsonHelper.TryLoadFromJson(configPath, out GameSavePathsConfig result))
            {
                if (!string.IsNullOrEmpty(result.StageProgressFilePath))
                {
                    _stageProgressFileName = result.StageProgressFilePath;
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
        }

        private static void SaveDefaultPaths(string configPath)
        {
            GameSavePathsConfig defaultConfig = new GameSavePathsConfig();
            JsonHelper.SaveToJson(defaultConfig, configPath);
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

