using UnityEngine;
using UnityEditor;
using System.IO;

namespace Characters.Common.Combat.Weapons.Editor
{
    public class DuplicateAndRenameTool : EditorWindow
    {
        private string _replacementText = "EtherealTempo";
        private int _partIndex;

        private int _displayedPartIndex = 2;

        // Add a menu item to open this window
        [MenuItem("Assets/Custom Tools/Duplicate and Replace Part")]
        public static void ShowWindow()
        {
            GetWindow<DuplicateAndRenameTool>("Duplicate & Rename");
        }

        private void OnGUI()
        {
            GUILayout.Label("Mass Duplicate & Rename", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (Selection.objects.Length == 0)
            {
                EditorGUILayout.HelpBox("Select one or more assets in the Project window.", MessageType.Warning);
                return;
            }

            GUILayout.Label($"Items selected: {Selection.objects.Length}");

            // Show a live preview based on the first selected item
            string firstObjectName = Selection.objects[0].name;
            string[] parts = firstObjectName.Split('_');

            if (parts.Length >= _partIndex + 1)
            {
                parts[_partIndex] = _replacementText;
                string previewName = string.Join("_", parts);
                EditorGUILayout.HelpBox($"Preview: \n{firstObjectName} \n-> \n{previewName}", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("First selected item does not have at least 3 parts separated by '_'.", MessageType.Error);
            }

            EditorGUILayout.Space();

            if(int.TryParse(EditorGUILayout.TextField("Part index", _displayedPartIndex.ToString()), out var number))
            {
                _displayedPartIndex = number;
            }
            _partIndex = _displayedPartIndex - 1;
            _replacementText = EditorGUILayout.TextField($"New {_partIndex + 1}rd Part Text:", _replacementText);

            EditorGUILayout.Space();

            if (GUILayout.Button("Duplicate Selected Assets", GUILayout.Height(30)))
            {
                ProcessSelectedFiles();
            }
        }

        private void ProcessSelectedFiles()
        {
            int successCount = 0;

            foreach (Object obj in Selection.objects)
            {
                string oldAssetPath = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(oldAssetPath)) continue;

                string oldName = Path.GetFileNameWithoutExtension(oldAssetPath);
                string extension = Path.GetExtension(oldAssetPath);
                string directory = Path.GetDirectoryName(oldAssetPath);

                string[] parts = oldName.Split('_');

                // Ensure the file name has at least 3 parts (index 0, 1, 2)
                if (parts.Length >= _partIndex + 1)
                {
                    parts[_partIndex] = _replacementText;
                    string newName = string.Join("_", parts);

                    // Generate Unique path prevents overwriting if a file with this name already exists
                    string newAssetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(directory, newName + extension));

                    if (AssetDatabase.CopyAsset(oldAssetPath, newAssetPath))
                    {
                        successCount++;
                    }
                }
                else
                {
                    Debug.LogWarning($"Skipped '{oldName}': Name does not contain enough '_' characters.");
                }
            }

            // Save and refresh the Asset Database to show new files
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Successfully duplicated and renamed {successCount} assets.");
        }
    }
}
