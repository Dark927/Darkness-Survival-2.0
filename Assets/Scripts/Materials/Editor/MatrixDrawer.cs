#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif

using UnityEngine;
using System.Linq;
using System;
using Materials;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Matrix4x4))]
public class Matrix4x4Drawer : PropertyDrawer
{
    private static readonly string[] s_defaultRows = { "X'", "Y'", "Z'", "W'" };
    private static readonly string[] s_defaultCols = { "X", "Y", "Z", "W" };

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement();

        // Load styles
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
            "Assets/Scripts/Materials/Editor/DefaultCommonDark.uss");
        root.styleSheets.Add(styleSheet);

        // Create field label
        var fieldLabel = new Label(property.displayName);
        fieldLabel.AddToClassList("field-label");
        root.Add(fieldLabel);

        // Create matrix grid
        var matrixContainer = CreateMatrixGrid(property);
        root.Add(matrixContainer);

        return root;
    }

    private void SetMatrix(SerializedProperty prop, Matrix4x4 matrix)
    {
        for (var row = 0; row < 4; row++)
        {
            for (var col = 0; col < 4; col++)
            {
                prop.FindPropertyRelative($"e{row}{col}").floatValue = matrix[row, col];
            }
        }
        prop.serializedObject.ApplyModifiedProperties();
    }

    private VisualElement CreateMatrixGrid(SerializedProperty matrixProperty)
    {
        var container = new VisualElement();
        container.AddToClassList("matrix-container");

        // Get row and column labels from attribute or use defaults
        string[] rowLabels;
        string[] colLabels;

        MatrixLabelsAttribute? matrixLabelsAttribute = null;
        var attributes = fieldInfo.GetCustomAttributes(typeof(MatrixLabelsAttribute), false);
        if (attributes.Length > 0)
        {
            matrixLabelsAttribute = attributes[0] as MatrixLabelsAttribute;
        }

        if (matrixLabelsAttribute != null)
        {
            if (matrixLabelsAttribute.Rows.Length != 4)
            {
                throw new ArgumentException("MatrixLabelsAttribute must have exactly 4 rows.");
            }
            rowLabels = matrixLabelsAttribute.Rows.Select(x => x + "'").ToArray();
            colLabels = matrixLabelsAttribute.Rows;
        }
        else
        {
            rowLabels = s_defaultRows;
            colLabels = s_defaultCols;
        }

        // Create header
        var header = new VisualElement();
        header.style.flexDirection = FlexDirection.Row;
        header.AddToClassList("matrix-header");

        // row/column filler
        var rowfiller = new Label();
        rowfiller.AddToClassList("row-label");
        header.Add(rowfiller);

        // Create column labels
        foreach (var col in colLabels)
        {
            var lbl = new Label(col);
            lbl.AddToClassList("col-label");
            header.Add(lbl);
        }
        container.Add(header);

        // Create clear button
        var resetBtn = new Button();
        resetBtn.text = "Reset";
        resetBtn.AddToClassList("reset-button");
        resetBtn.clicked += () => ResetMatrix(matrixProperty);
        header.Add(resetBtn);

        // Create rows
        for (var row = 0; row < 4; row++)
        {
            var rowContainer = new VisualElement();
            rowContainer.AddToClassList("row-container");

            // Row label
            var lbl = new Label(rowLabels[row]);
            lbl.AddToClassList("row-label");
            rowContainer.Add(lbl);

            // Matrix cells
            for (var col = 0; col < 4; col++)
            {
                var cell = new FloatField();
                cell.AddToClassList("small-label");
                cell.label = $"M{row}{col}";
                cell.bindingPath = $"{matrixProperty.propertyPath}.e{row}{col}";
                rowContainer.Add(cell);
            }

            container.Add(rowContainer);
        }
        container.Bind(matrixProperty.serializedObject);
        return container;
    }

    private void ResetMatrix(SerializedProperty matrixProperty)
    {
        // Write back entire matrix
        SetMatrix(matrixProperty, Matrix4x4.identity);
    }
}
#endif
