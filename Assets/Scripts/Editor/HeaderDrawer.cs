using UnityEditor;
using UnityEngine;

namespace Utilities.Attributes
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CustomHeaderAttribute))]
    public class HeaderDrawer : DecoratorDrawer
    {
        const float spacing = 8f;
        const float padding = 2f;
        const float margin = -20f;
        const float barHeight = 4f;

        public override void OnGUI(Rect position)
        {
            var attr = (CustomHeaderAttribute)attribute;
            var pos = EditorGUI.IndentedRect(position);
            var rowHeight = (EditorGUIUtility.singleLineHeight * attr.count) + (EditorGUIUtility.standardVerticalSpacing * attr.count);

            var headerRect = new Rect(pos.x + margin, pos.y + spacing, (pos.width - margin) + (padding * 2), pos.height - (spacing + barHeight + spacing));
            EditorGUI.DrawRect(headerRect, Constants.BackgroundColor);

            var customLabelStyle = new GUIStyle(Constants.LabelStyle);
            customLabelStyle.normal.textColor = attr.headerColor;

            EditorGUI.LabelField(headerRect, new GUIContent(attr.label, attr.tooltip), customLabelStyle);

            // only draw bar and child background if this header has children
            if (attr.count > 0)
            {
                // draw depth color bar
                var barRect = new Rect(headerRect.x, headerRect.y + headerRect.height + (spacing / 2), headerRect.width, barHeight);
                EditorGUI.DrawRect(barRect, Constants.ColorForDepth(attr.depth));

                // draw child background
                var childrenRect = new Rect(headerRect.x, position.y + position.height - padding, headerRect.width + (padding * 2), rowHeight + (padding * 2));
                EditorGUI.DrawRect(childrenRect, Constants.BackgroundColor);
            }
        }

        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight * 2.5f;
        }
#endif

        public static class Constants
        {
            private static readonly Color[] _barColors = new Color[5] {
                new Color(0.3411765f, 0.6039216f, 0.7803922f),
                new Color(0.145098f, 0.6f, 0.509804f),
                new Color(0.9215686f, 0.6431373f, 0.282353f),
                new Color(0.8823529f, 0.3529412f, 0.4039216f),
                new Color(0.9529412f, 0.9294118f, 0.682353f)
            };

            public static Color ColorForDepth(int depth) => _barColors[depth % _barColors.Length];

            public static Color BackgroundColor { get; } = EditorGUIUtility.isProSkin ? new Color(0.18f, 0.18f, 0.18f, 0.75f) : new Color(0.82f, 0.82f, 0.82f, 0.75f);

            public static GUIStyle LabelStyle { get; } = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
        }
    }
}

