using UnityEditor;
using UnityEngine;
using Dark.Utils;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

namespace Dark.Tile
{
    public class NormalProcessorArrayWindow : EditorWindow
    {
        private static List<(float, float)> args;// smoothness/intensity
        private int _gridSize = 8; // Number of textures per row
        private static int _selected = 0;
        private Texture2DArray textureArray;
        private Vector2 scrollPosition;

        #region menu
        [MenuItem("Assets/Dark/Dark Tile Editor")]
        public static void ShowWindow()
        {
            GetWindow<NormalProcessorArrayWindow>("Dark Tile Editor");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            #region tile chooser
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));

            textureArray = (Texture2DArray)EditorGUILayout.ObjectField("Texture2DArray", textureArray, typeof(Texture2DArray), false, GUILayout.ExpandWidth(false));

            if (textureArray == null)
            {
                EditorGUILayout.HelpBox("Please assign a Texture2DArray to view its contents.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField("Grid Size (Columns)");

            _gridSize = EditorGUILayout.IntSlider(_gridSize, 1, 8, GUILayout.Width(64 * 4));
            // Scrollable Grid View
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawTextureGrid();
            EditorGUILayout.EndScrollView();

            GUILayout.EndVertical();
            #endregion
            #region preview
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            //sliders
            args ??= new List<(float, float)>();
            args.Resize(textureArray.depth);

            args[_selected] = (
                EditorGUILayout.Slider("Smoothness", args[_selected].Item1, 1, 10),
                EditorGUILayout.Slider("Intensity", args[_selected].Item2, 1, 10)
            );

            // preview on the right side (now its on the bottom)
            GUILayout.Label("Preview", EditorStyles.boldLabel);

            GUI.DrawTexture(GUILayoutUtility.GetRect(256, 256), ConvertLayer(textureArray, _selected));
            GUILayout.EndVertical();
            #endregion

            EditorGUILayout.EndHorizontal();

        }

        private void DrawTextureGrid()
        {
            int totalLayers = textureArray.depth;

            //texture tile style
            GUIStyle textureStyle = new()
            {
                padding = new RectOffset(1, 1, 1, 1),
                stretchWidth = false
            };
            //center text
            GUIStyle textStyle = new(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter
            };

            // Iterate through all layers and draw them in a grid
            for (int i = 0; i < totalLayers; i += _gridSize)
            {
                GUILayout.BeginHorizontal();
                for (int j = 0; j < _gridSize && i + j < totalLayers; j++)
                {
                    int index = i + j;
                    Texture2D previewTexture = ExtractLayer(textureArray, index);

                    if (previewTexture != null)
                    {
                        GUILayout.BeginVertical(textureStyle);
                        Rect rect = GUILayoutUtility.GetRect(64, 64, textureStyle);
                        GUI.DrawTexture(rect, previewTexture);

                        GUI.Label(rect, $"{index}", textStyle);

                        if (index == _selected)
                        {
                            Color borderColor = Color.yellow;
                            Handles.DrawSolidRectangleWithOutline(rect, Color.clear, borderColor);
                        }

                        Event e = Event.current;
                        if (e.type == EventType.MouseDown && e.button == 0 && rect.Contains(e.mousePosition))
                        {
                            _selected = index;
                            Event.current.Use();// Consume the event to prevent multiple clicks
                        }
                        GUILayout.EndVertical();
                    }
                }
                GUILayout.EndHorizontal();
            }
        }

        private int arrayVersion; // Tracks changes to the array
        private int ComputeArrayVersion(Texture2DArray arr)
        {
            // Compute a version based on the array's properties (e.g., dimensions, format, or even a hash)
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + arr.width;
                hash = hash * 31 + arr.height;
                hash = hash * 31 + arr.GetHashCode();
                hash = hash * 31 + arr.depth;
                return hash;
            }
        }

        private static Dictionary<int, Texture2D> TextureCache { get; set; } = new Dictionary<int, Texture2D>();
        public Texture2D ExtractLayer(Texture2DArray array, int index)
        {
            // Check if the array has changed and invalidate the cache if necessary
            if (arrayVersion != ComputeArrayVersion(array))
            {
                TextureCache.Clear();
                arrayVersion = ComputeArrayVersion(array);
            }

            // Return cached texture if available
            if (TextureCache.TryGetValue(index, out Texture2D cachedTexture))
            {
                return cachedTexture;
            }

            // Create a new texture and cache it
            Texture2D tempTexture = new(array.width, array.height, array.format, false);
            Graphics.CopyTexture(array, index, 0, tempTexture, 0, 0);
            tempTexture.Apply(); // Ensure the texture is updated
            TextureCache[index] = tempTexture;

            return tempTexture;
        }

        private Texture2DArray CachedNormalMap;
        private int CachedNormalMap_Hash;

        public Texture2D ConvertLayer(Texture2DArray array, int index)
        {
            // Check if the array has changed and invalidate the cache if necessary
            var hash = HashCode.Combine(array.GetHashCode(), args[0].Item1.GetHashCode(), args[0].Item2.GetHashCode());
            if (CachedNormalMap_Hash != hash || CachedNormalMap == null)
            {
                using var processor = new NormalProcessorArray();
                CachedNormalMap = processor.ProcessTexture(array, smoothness: args[0].Item1, intensity: args[0].Item2);
                CachedNormalMap_Hash = hash;
            }


            Texture2D tempTexture = new(CachedNormalMap.width, CachedNormalMap.height, CachedNormalMap.format, false);
            Graphics.CopyTexture(CachedNormalMap, index, 0, tempTexture, 0, 0);
            tempTexture.Apply(); // Ensure the texture is updated
            return tempTexture;
        }
        #endregion
    }
}