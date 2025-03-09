/*
    Copyright © 2024 Vitiaz Denys (GitHub: arduinka55055)
    This code is part of Dark TileMap, a Unity asset owned by Vitiaz Denys.
    
    Purchase of this asset grants a non-exclusive, non-transferable license to use it
    in Unity projects. Redistribution, resale, or sharing of this code or the asset 
    in any form, including modified or unmodified, is strictly prohibited.
    
    For full license terms, refer to the LICENSE file included with this asset.
    
    Unauthorized use of this asset is a violation of copyright law and may result 
    in legal action.
*/

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;

namespace Dark.Tile
{
    [CustomEditor(typeof(DarkTileMap))]
    public class AssetUtil : Editor
    {
        public static void CreateAsset<TileMapData>() where TileMapData : ScriptableObject
        {
            TileMapData asset = ScriptableObject.CreateInstance<TileMapData>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(TileMapData).ToString() + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        public override void OnInspectorGUI()
        {
            DarkTileMap e = (DarkTileMap)target;

            EditorGUI.BeginChangeCheck();

            e.Size = EditorGUILayout.Vector2IntField(
                "Size",
                e.Size
            );
            e.TextureAtlas = (Texture2DArray)
                    EditorGUILayout.ObjectField(
                        "Texture Atlas Array",
                        e.TextureAtlas,
                        typeof(Texture2DArray),
                        false
                    );

            e.previewImage = (Texture2D)
                    EditorGUILayout.ObjectField(
                        "Thumbnail",
                        e.previewImage,
                        typeof(Texture2D),
                        false
                    );

            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty uniformBufferProperty = serializedObject.FindProperty("UniformBuffer");

            EditorGUILayout.PropertyField(uniformBufferProperty, true);

            if (GUILayout.Button("Open Tile Map Editor"))
            {
                MenuUtils.OpenPreview();
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(e.TextureAtlas);
                AssetDatabase.SaveAssets();
                Repaint();
            }
        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            DarkTileMap TileMapData = (DarkTileMap)target;

            if (TileMapData == null || TileMapData.previewImage == null)
                return null;

            // TileMapData.PreviewIcon must be a supported format: ARGB32, RGBA32, RGB24,
            // Alpha8 or one of float formats
            Texture2D tex = new(width, height);
            EditorUtility.CopySerialized(TileMapData.previewImage, tex);

            return tex;
        }

        #region double click opens editor
        private void OnEnable()
        {
            // Attach the double-click handler when the asset is selected
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        }

        private void OnDisable()
        {
            // Detach the handler when this editor is disabled
            EditorApplication.projectWindowItemOnGUI -= OnProjectWindowItemGUI;
        }
        private void OnProjectWindowItemGUI(string guid, Rect selectionRect)
        {
            // Check if the selected asset is a TileMapData
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<DarkTileMap>(assetPath);

            if (asset != null && Event.current != null && Event.current.type == EventType.MouseDown &&
                Event.current.clickCount == 2 && selectionRect.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                MenuUtils.OpenPreview();
            }
        }
        #endregion
    }
}
