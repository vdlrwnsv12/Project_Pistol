using UnityEngine;

/// <summary>
/// 방의 끝 지점을 나타내는 컴포넌트입니다.
/// 방향을 Enum으로 선택하여 설정합니다.
/// </summary>
public class EndPoint : MonoBehaviour
{
    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        Up,
        Down
    }

    [Tooltip("다음 방이 나아갈 방향을 선택하세요.")]
    public Direction direction = Direction.Forward;

    /// <summary>
    /// 선택된 방향을 Vector3로 변환하여 반환합니다.
    /// </summary>
    public Vector3 GetManualForward()
    {
        switch (direction)
        {
            case Direction.Forward:
                return Vector3.forward;  // (0,0,1)
            case Direction.Backward:
                return Vector3.back;     // (0,0,-1)
            case Direction.Left:
                return Vector3.left;     // (-1,0,0)
            case Direction.Right:
                return Vector3.right;    // (1,0,0)
            case Direction.Up:
                return Vector3.up;       // (0,1,0)
            case Direction.Down:
                return Vector3.down;     // (0,-1,0)
            default:
                return Vector3.forward;
        }
    }
}
