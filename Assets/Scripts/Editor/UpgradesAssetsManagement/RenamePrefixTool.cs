using UnityEngine;
using UnityEditor;
using System.IO;

namespace Characters.Common.Combat.Weapons.Editor
{
    public class RenamePrefixTool : EditorWindow
    {
        private string _newPrefix = "Upgrade_Single_VoidPresence";

        [MenuItem("Assets/Custom Tools/Rename Prefix (Keep _value_SO)")]
        public static void ShowWindow()
        {
            GetWindow<RenamePrefixTool>("Rename Prefix");
        }

        private void OnGUI()
        {
            GUILayout.Label("Mass Rename (Keep _value_SO)", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (Selection.objects.Length == 0)
            {
                EditorGUILayout.HelpBox("Select one or more assets in the Project window.", MessageType.Warning);
                return;
            }

            GUILayout.Label($"Items selected: {Selection.objects.Length}");

            string firstObjectName = Selection.objects[0].name;
            string[] parts = firstObjectName.Split('_');

            if (parts.Length >= 3)
            {
                string valuePart = parts[parts.Length - 2]; 
                string soPart = parts[parts.Length - 1];
                string previewName = $"{_newPrefix}_{valuePart}_{soPart}";

                EditorGUILayout.HelpBox($"Preview: \n{firstObjectName} \n-> \n{previewName}", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("The selected file name does not end with _value_SO format.", MessageType.Error);
            }

            EditorGUILayout.Space();

            _newPrefix = EditorGUILayout.TextField("New Prefix:", _newPrefix);

            EditorGUILayout.Space();

            if (GUILayout.Button("Rename Selected Assets", GUILayout.Height(30)))
            {
                ProcessRename();
            }
        }

        private void ProcessRename()
        {
            int successCount = 0;

            foreach (Object obj in Selection.objects)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(assetPath)) continue;

                string oldName = Path.GetFileNameWithoutExtension(assetPath);
                string[] parts = oldName.Split('_');

                if (parts.Length >= 3)
                {
                    string valuePart = parts[parts.Length - 2];
                    string soPart = parts[parts.Length - 1];
                    string newName = $"{_newPrefix}_{valuePart}_{soPart}";

                    string error = AssetDatabase.RenameAsset(assetPath, newName);

                    if (string.IsNullOrEmpty(error))
                    {
                        successCount++;
                    }
                    else
                    {
                        Debug.LogError($"Failed to rename '{oldName}': {error}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Skipped '{oldName}': Format is not correct (missing _value_SO).");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Successfully renamed {successCount} assets.");
        }
    }
}
