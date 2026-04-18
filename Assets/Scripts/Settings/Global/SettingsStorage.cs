using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Utilities.Json;

namespace Settings.Global
{
    public class SettingsStorage : ISettingsStorage
    {
        private readonly string _savePath = Path.Combine(Application.persistentDataPath, "Settings", "global_game_settings.json");

        // The Single Source of Truth
        public GameSettingsData Data { get; private set; } = new GameSettingsData();
        public bool IsLoaded { get; private set; } = false;

        public void Initialize()
        {
            LoadSettingsAsync().Forget();
        }

        private async UniTask LoadSettingsAsync()
        {
            var (success, result) = await JsonHelper.TryLoadFromJsonAsync<GameSettingsData>(_savePath);

            if (success && result != null)
            {
                Data = result;
            }

            // Flag that it is safe for Audio/Input/Graphics to read the data
            IsLoaded = true;
        }

        public void SaveAllSettings()
        {
            JsonHelper.SaveToJsonAsync(Data, _savePath).Forget();
        }
    }
}
