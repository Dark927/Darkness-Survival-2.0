using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Utilities.Json
{
    public static class JsonHelper
    {
        public static void SaveToJson(object obj, string saveFilePath, bool forceCreateFolders = true)
        {
            try
            {
                if (forceCreateFolders && !Directory.Exists(Path.GetDirectoryName(saveFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(saveFilePath));
                }

                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText(saveFilePath, json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save stage progress: {e.Message}");
            }
        }

        public static bool TryLoadFromJson<T>(string filePath, out T result)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    result = JsonConvert.DeserializeObject<T>(json);
                    return true;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to load stage progress: {e.Message}");
                }
            }

            result = default;
            return false;
        }
    }
}
