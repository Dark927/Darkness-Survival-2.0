using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Characters.Player.Upgrades.Editor
{
    [CustomEditor(typeof(UpgradeDataSO), true)]
    public class UpgradeSOEditor : UnityEditor.Editor
    {
        // Layout Configurations
        private const float OuterMargin = 20f;
        private const float InnerPadding = 10f;

        // Color Configurations
        private readonly Color _cardBackgroundColor = new Color(0.155f, 0.237f, 0.252f, 1f);
        private readonly Color _selectionHighlightColor = new Color(0.17f, 0.36f, 0.53f, 1f);

        private SerializedProperty _upgradeLevelsProp;
        private ReorderableList _reorderableList;

        private void OnEnable()
        {
            _upgradeLevelsProp = serializedObject.FindProperty("_upgradeLevels");

            if (_upgradeLevelsProp != null)
            {
                _reorderableList = new ReorderableList(serializedObject, _upgradeLevelsProp, true, true, true, true);

                _reorderableList.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Upgrade Levels (Drag to Reorder)", EditorStyles.boldLabel);
                };

                // Handle the background drawing to create visual separation
                _reorderableList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    ApplyOuterMargin(ref rect);

                    // Draw selection highlight if the element is active/focused
                    if (isActive || isFocused)
                    {
                        EditorGUI.DrawRect(rect, _selectionHighlightColor);
                    }
                };

                // Handle the rendering of the card and the properties inside it
                _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    SerializedProperty element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);

                    ApplyOuterMargin(ref rect);

                    // Render card base and border
                    EditorGUI.DrawRect(rect, _cardBackgroundColor);
                    GUI.Box(rect, GUIContent.none, EditorStyles.helpBox);

                    // Calculate inner content bounds
                    Rect contentRect = new Rect(
                        rect.x + InnerPadding,
                        rect.y + InnerPadding,
                        rect.width - (InnerPadding * 2f),
                        rect.height - (InnerPadding * 2f)
                    );

                    GUIContent customLabel = new GUIContent($"Level {index + 1}");
                    EditorGUI.PropertyField(contentRect, element, customLabel, true);
                };

                // Calculate dynamic height based on the property state and layout configuration
                _reorderableList.elementHeightCallback = (int index) =>
                {
                    SerializedProperty element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);

                    float baseHeight = EditorGUI.GetPropertyHeight(element, true);
                    float layoutOverhead = (OuterMargin * 2f) + (InnerPadding * 2f);

                    return baseHeight + layoutOverhead;
                };
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "_upgradeLevels");

            if (_upgradeLevelsProp != null)
            {
                EditorGUILayout.Space(20);
                _reorderableList.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Mutates the provided rect to apply vertical spacing between list elements.
        /// </summary>
        private void ApplyOuterMargin(ref Rect rect)
        {
            rect.y += OuterMargin;
            rect.height -= (OuterMargin * 2f);
        }
    }
}
