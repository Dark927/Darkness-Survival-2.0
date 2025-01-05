using UnityEditor;
using UnityEngine;
using Dark.Utils;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Dark.Tile
{

    public class DarkTileEdit : EditorWindow
    {
        private int _gridSize = 8; // Number of textures per row
        private static int _selected = -1;
        private Texture2DArray textureArray;
        private Vector2 scrollPosition;

        #region menu
        [MenuItem("Window/Dark Tile Editor")]
        public static void ShowWindow()
        {
            GetWindow<DarkTileEdit>("Dark Tile Editor");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Dark Tile Editor", EditorStyles.boldLabel);
            textureArray = (Texture2DArray)EditorGUILayout.ObjectField("Texture2DArray", textureArray, typeof(Texture2DArray), false);

            if (textureArray == null)
            {
                EditorGUILayout.HelpBox("Please assign a Texture2DArray to view its contents.", MessageType.Info);
                return;
            }

            _gridSize = Mathf.Clamp(EditorGUILayout.IntField("Grid Size (Columns)", _gridSize), 1, 10);

            // Scrollable Grid View
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawTextureGrid();
            EditorGUILayout.EndScrollView();
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

        private Texture2D ExtractLayer(Texture2DArray array, int index)
        {
            // Create a temporary Texture2D
            Texture2D tempTexture = new(array.width, array.height, array.format, false);
            Graphics.CopyTexture(array, index, 0, tempTexture, 0, 0);
            tempTexture.Apply(); // Ensure the texture is updated
            return tempTexture;
        }
        #endregion

        #region scene
        [InitializeOnLoadMethod]
        private static void EnableSceneGUI()
        {
            SceneView.duringSceneGui += (sceneView) =>
            {

                //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                var e = Event.current;

                if (!EditorWindow.HasOpenInstances<DarkTileEdit>()) return;
                if (_selected == -1) return;

                //cancel with right click or keyboard
                if (e.button == 1 || e.button == 2 || e.type == EventType.KeyDown)
                {
                    _selected = -1;
                }
                //left click or drag with window opened
                if (e.button == 0 && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag))
                {
                    var a = HandleUtility.PickGameObject(e.mousePosition, false);

                    DarkTileEdit window = EditorWindow.GetWindow<DarkTileEdit>();
                    if(window != null && sceneView != null && a?.scene != null)
                    window.OnSceneGUI(sceneView, a.scene);
                }
            };
        }
        private void OnSceneGUI(SceneView sceneView, Scene sceneForRaycast)
        {
            if (_selected != -1)
            {
                Camera sceneCamera = SceneView.lastActiveSceneView.camera;
                if (sceneCamera == null) return;

                // Perform raycast
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                PhysicsScene physicsScene = sceneForRaycast.GetPhysicsScene();

                if (physicsScene.IsValid() && physicsScene.Raycast(ray.origin, ray.direction, out RaycastHit hit))
                {
                    DarkTileMapDraw tileScript = hit.collider.GetComponent<DarkTileMapDraw>();
                    if (tileScript == null || textureArray == null) return;

                    // Call your tile editing logic
                    tileScript.OnDarkTileEdit(hit, _selected);
                }
                Event.current.Use(); // Consume the event
                SceneView.RepaintAll(); // Ensure Scene View updates
            }
        }
        private void OnSceneGUICancel(SceneView sceneView)
        {
            _selected = -1; // Reset selected tile index
        }

        #endregion
    }
}