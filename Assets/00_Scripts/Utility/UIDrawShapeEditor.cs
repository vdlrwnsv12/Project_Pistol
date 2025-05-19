using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIDrawShape))]
public class UIDrawShapeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIDrawShape shape = (UIDrawShape)target;

        shape.shape = (UIDrawShape.ShapeType)EditorGUILayout.EnumPopup("Shape Type", shape.shape);
        shape.size = EditorGUILayout.Vector2Field("Size", shape.size);

        if (shape.shape == UIDrawShape.ShapeType.Parallelogram)
        {
            shape.skew = EditorGUILayout.Slider("Skew", shape.skew, 0f, 1f);
        }
        else if (shape.shape == UIDrawShape.ShapeType.Trapezoid)
        {
            shape.topWidthRatio = EditorGUILayout.Slider("Top Width Ratio", shape.topWidthRatio, 0f, 1f);
            shape.bottomWidthRatio = EditorGUILayout.Slider("Bottom Width Ratio", shape.bottomWidthRatio, 0f, 1f);
        }

        shape.color = EditorGUILayout.ColorField("Color", shape.color);

        if (GUI.changed)
        {
            shape.SetVerticesDirty();
        }
    }
}
