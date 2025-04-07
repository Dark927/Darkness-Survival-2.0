using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Materials;

[CustomPropertyDrawer(typeof(ITogglableMaterialProps), true)]
public class ITogglableMaterialPropsDrawerUIE : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        SerializedProperty enabledProp = property.FindPropertyRelative("_enabled");
        if (enabledProp == null)
        {
            Debug.LogError($"No field '_enabled' found in ITogglableMaterialProps implementer ({property.type})!");
            return container;
        }

        // Header row with toggle and foldout
        var headerRow = new VisualElement();
        headerRow.style.flexDirection = FlexDirection.Row;
        headerRow.style.alignItems = Align.FlexStart;

        // Enable toggle
        var enableToggle = new Toggle();
        enableToggle.BindProperty(enabledProp);
        enableToggle.style.marginRight = 15;
        enableToggle.style.marginTop = 3;
        headerRow.Add(enableToggle);

        // Foldout for expansion control
        var foldout = new Foldout
        {
            text = property.displayName,
            value = property.isExpanded,
        };
        foldout.style.flexGrow = 1;


        headerRow.Add(foldout);
        container.Add(headerRow);

        // Content container for child properties
        var contentContainer = new VisualElement();
        contentContainer.style.marginLeft = 15;
        contentContainer.style.display = property.isExpanded ? DisplayStyle.Flex : DisplayStyle.None;
        container.Add(contentContainer);

        // Populate child properties
        foreach (var child in GetChildren(property))
        {
            if (child.name == "_enabled") continue;

            var childField = new PropertyField(child);
            contentContainer.Add(childField);
        }

        // Foldout toggle callback
        foldout.RegisterValueChangedCallback(evt =>
        {
            contentContainer.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            property.isExpanded = evt.newValue;
            property.serializedObject.ApplyModifiedProperties();
        });

        // Enable toggle callback
        enableToggle.RegisterValueChangedCallback(evt =>
        {
            contentContainer.SetEnabled(evt.newValue);
        });

        return container;
    }

    private static IEnumerable<SerializedProperty> GetChildren(SerializedProperty property)
    {
        var iterator = property.Copy();
        var end = iterator.GetEndProperty();
        iterator.NextVisible(true);

        while (!SerializedProperty.EqualContents(iterator, end))
        {
            yield return iterator;
            iterator.NextVisible(false);
        }
    }
}
