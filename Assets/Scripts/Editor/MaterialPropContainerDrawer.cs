using System;
using System.Reflection;
using Materials;
using UnityEditor;
using UnityEngine;


/// Unity Editor helper class
/// Triggers changes for a MaterialPropContainer's Property setter
/// It uses reflection, and marks changes MaterialPropContainer
/// Not needed in runtime

namespace Dark.EditorUtils
{
    [CustomPropertyDrawer(typeof(MaterialPropContainer<>), true)]
    public class MaterialPropContainerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Begin drawing the property.
            EditorGUI.BeginProperty(position, label, property);

            // Remember the height
            float yOffset = position.y;

            // Draw a foldout header.
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label);
            yOffset += EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;

            // Draw the _constMaterialProps field (optional)
            var scriptableObjectProp = property.FindPropertyRelative("_constMaterialProps");
            if (scriptableObjectProp != null)
            {
                var fieldRect2 = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(fieldRect2, scriptableObjectProp, true);

                yOffset += EditorGUI.GetPropertyHeight(scriptableObjectProp, true);
            }


            // Get the SerializedProperty for the backing field "_properties"
            var propertiesProp = property.FindPropertyRelative("_properties");
            if (propertiesProp == null)
            {
                EditorGUI.LabelField(
                    new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight),
                    "No _properties field found");
                EditorGUI.indentLevel--;
                EditorGUI.EndProperty();
                return;
            }

            // Calculate rect for drawing
            var fieldRect = new Rect(
                position.x,
                yOffset,
                position.width,
                EditorGUI.GetPropertyHeight(propertiesProp, true)
            );

            // Wrap drawing in a change check.
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(fieldRect, propertiesProp, true);
            if (EditorGUI.EndChangeCheck())
            {
                // Changes have been detected; now update the container.
                var container = ReflectionUtility.GetTargetObjectOfProperty(property);
                if (container == null)
                {
                    Debug.LogError("Container is null");
                    return;
                }

                // Use reflection to get the "Properties" property, and _properties backing field
                var containerType = container.GetType();
                var propInfo = containerType.GetProperty("Properties", BindingFlags.Public | BindingFlags.Instance);
                if (propInfo == null)
                {
                    Debug.LogError("No 'Properties' property found on container type: " + containerType.Name);
                    return;
                }
                var backingField = containerType.GetField("_properties", BindingFlags.NonPublic | BindingFlags.Instance);
                if (backingField == null)
                {
                    Debug.LogError("No '_properties' field found on container type: " + containerType.Name);
                    return;
                }

                var newPropertiesValue = backingField.GetValue(container);
                // Calling the setter
                propInfo.SetValue(container, newPropertiesValue);
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight; // Height for the foldout header.

            if (property.isExpanded)
            {
                var scriptableObjectProp = property.FindPropertyRelative("_constMaterialProps");
                if (scriptableObjectProp != null)
                {
                    height += EditorGUI.GetPropertyHeight(scriptableObjectProp, true);
                }

                var propertiesProp = property.FindPropertyRelative("_properties");
                if (propertiesProp != null)
                {
                    height += EditorGUI.GetPropertyHeight(propertiesProp, true);
                }
            }
            return height;
        }
    }
}
