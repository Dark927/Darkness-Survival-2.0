using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindAssetReferences : EditorWindow
{
    [MenuItem("Assets/Custom Tools/Find References In Project", false, 25)]
    private static void FindReferences()
    {
        if (Selection.activeObject == null) return;

        string selectedAssetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(selectedAssetPath)) return;

        Debug.Log($"<b>Searching for references to:</b> {selectedAssetPath}");

        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        int referenceCount = 0;

        foreach (string path in allAssetPaths)
        {
            // Skip the asset itself
            if (path == selectedAssetPath) continue;

            // GetDependencies returns all assets that the given asset depends on
            string[] dependencies = AssetDatabase.GetDependencies(path, false);

            if (dependencies.Contains(selectedAssetPath))
            {
                Object referencingObject = AssetDatabase.LoadMainAssetAtPath(path);
                Debug.Log($"<color=cyan>Referenced by:</color> {path}", referencingObject);
                referenceCount++;
            }
        }

        if (referenceCount == 0)
        {
            Debug.Log("<color=green>No references found! Safe to delete.</color>");
        }
        else
        {
            Debug.Log($"<b>Found {referenceCount} references.</b> Check the logs above.");
        }
    }
}
