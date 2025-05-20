using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIDrawShape : MaskableGraphic
{
    public enum ShapeType { Rectangle, Parallelogram, Trapezoid }
    public ShapeType shape = ShapeType.Rectangle;

    [Range(0f, 1f)] public float skew = 0.2f; // 평행사변형 기울기

    [Range(0f, 1f)] public float topWidthRatio = 0.7f;     // 사다리꼴 상단 너비 비율
    [Range(0f, 1f)] public float bottomWidthRatio = 1f;    // 사다리꼴 하단 너비 비율

    public Vector2 size = new Vector2(200, 100);

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Vector2 half = size * 0.5f;

        Vector2 topLeft, topRight, bottomRight, bottomLeft;

        switch (shape)
        {
            case ShapeType.Parallelogram:
            {
                float skewOffset = skew * size.y;

                topLeft     = new Vector2(-half.x + skewOffset, half.y);
                topRight    = new Vector2(half.x + skewOffset, half.y);
                bottomRight = new Vector2(half.x - skewOffset, -half.y);
                bottomLeft  = new Vector2(-half.x - skewOffset, -half.y);
                break;
            }
            case ShapeType.Trapezoid:
            {
                float topHalfWidth = (size.x * topWidthRatio) * 0.5f;
                float bottomHalfWidth = (size.x * bottomWidthRatio) * 0.5f;

                topLeft     = new Vector2(-topHalfWidth, half.y);
                topRight    = new Vector2(topHalfWidth, half.y);
                bottomRight = new Vector2(bottomHalfWidth, -half.y);
                bottomLeft  = new Vector2(-bottomHalfWidth, -half.y);
                break;
            }
            default: // Rectangle
            {
                topLeft     = new Vector2(-half.x, half.y);
                topRight    = new Vector2(half.x, half.y);
                bottomRight = new Vector2(half.x, -half.y);
                bottomLeft  = new Vector2(-half.x, -half.y);
                break;
            }
        }

        vh.AddVert(topLeft, color, Vector2.zero);
        vh.AddVert(topRight, color, Vector2.zero);
        vh.AddVert(bottomRight, color, Vector2.zero);
        vh.AddVert(bottomLeft, color, Vector2.zero);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}
