using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Utilities.ErrorHandling;

namespace Utilities.Json
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private static readonly object _fileLock = new object();

        public static async UniTask<bool> SaveToJsonAsync(object obj, string saveFilePath, bool forceCreateFolders = true, CancellationToken token = default)
        {
            try
            {
                if (forceCreateFolders)
                {
                    string directory = Path.GetDirectoryName(saveFilePath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        await UniTask.RunOnThreadPool(() => Directory.CreateDirectory(directory), cancellationToken: token);
                    }
                }

                string json = await UniTask.RunOnThreadPool(() => JsonConvert.SerializeObject(obj, _settings), cancellationToken: token);

                await UniTask.RunOnThreadPool(() =>
                {
                    lock (_fileLock)
                    {
                        File.WriteAllText(saveFilePath, json);
                    }
                }, cancellationToken: token);


                return true;
            }
            catch (OperationCanceledException)
            {
                ErrorLogger.Log("Save cancelled");
                return false;
            }
            catch (Exception e)
            {
                ErrorLogger.LogWarning($"Save failed: {e.Message}");
                return false;
            }
        }

        public static async UniTask<(bool success, T result)> TryLoadFromJsonAsync<T>(string filePath, CancellationToken token = default)
        {
            if (!File.Exists(filePath))
            {
                return (false, default);
            }

            try
            {
                string json = await UniTask.RunOnThreadPool(() => File.ReadAllText(filePath), cancellationToken: token);

                T result = await UniTask.RunOnThreadPool(() => JsonConvert.DeserializeObject<T>(json, _settings), cancellationToken: token);

                return (result != null, result);
            }
            catch (OperationCanceledException)
            {
                ErrorLogger.Log("Load cancelled");
                return (false, default);
            }
            catch (Exception e)
            {
                ErrorLogger.LogWarning($"Load failed: {e.Message}");
                return (false, default);
            }
        }
    }
}
