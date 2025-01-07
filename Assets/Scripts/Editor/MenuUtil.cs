using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace Dark.Tile
{
    public static class MenuUtils
    {
        [MenuItem("Assets/Open Dark Tile Preview", true)]
        public static bool ValidateOpenPreview()
        {
            return Selection.activeObject is DarkTileMap;
        }

        [MenuItem("Assets/Open Dark Tile Preview")]
        public static void OpenPreview()
        {
            DarkTileMap selectedTileMap = Selection.activeObject as DarkTileMap;
            if (selectedTileMap == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select a valid Tile Map asset.", "OK");
                return;
            }

            // Open the custom preview stage
            var stage = ScriptableObject.CreateInstance<PreviewStage>();
            //stage.SetTileMap(selectedTileMap);
            StageUtility.GoToStage(stage, true);
            stage.CreatePreviewQuad();
        }
    }
}
