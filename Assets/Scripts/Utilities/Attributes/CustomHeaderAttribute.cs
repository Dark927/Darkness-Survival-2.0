using System;
using UnityEngine;

namespace Utilities.Attributes
{
    public class CustomHeaderAttribute : PropertyAttribute
    {
        public enum HeaderColor
        {
            defaultColor = 0,
            green,
            cyan,
            yellow,
            red
        }

        public int count;
        public int depth;
        public string label;
        public string tooltip;
        public HeaderColor headerColor;

        public Color ConcreteHeaderColor => GetColor(headerColor);

        /// <summary>
        /// Add a header above a field
        /// </summary>
        /// <param name="label">A title for the header label</param>
        /// <param name="count">the number of child elements under this header</param>
        /// <param name="depth">the depth of this header element in the inspector foldout</param>
        /// <param name="headerColor">optional custom header color</param>
        public CustomHeaderAttribute(string label, int count = default, int depth = default, HeaderColor headerColor = HeaderColor.defaultColor)
        {
            this.count = count;
            this.depth = depth;
            this.label = label;
            this.headerColor = headerColor;
        }

        /// <summary>
        /// Add a header above a field with a tooltip
        /// </summary>
        /// <param name="label">A title for the header label</param>
        /// <param name="tooltip">A note or instruction shown when hovering over the header</param>
        /// <param name="count">the number of child elements under this header</param>
        /// <param name="depth">the depth of this header element in the inspector foldout</param>
        /// <param name="headerColor">optional custom header color</param>
        public CustomHeaderAttribute(string label, string tooltip, int count = default, int depth = default, HeaderColor headerColor = HeaderColor.defaultColor)
        {
            this.count = count;
            this.depth = depth;
            this.label = label;
            this.tooltip = tooltip;
            this.headerColor = headerColor;
        }

        public static Color GetColor(HeaderColor colorType)
        {
            return colorType switch
            {
                HeaderColor.defaultColor => Color.white,
                HeaderColor.green => Color.green,
                HeaderColor.cyan => Color.cyan,
                HeaderColor.yellow => Color.yellow,
                HeaderColor.red => Color.red,
                _ => throw new NotImplementedException()
            };
        }
    }
}
