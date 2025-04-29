using UnityEngine;

/// <summary>
/// 방의 끝 지점입니다.  
/// 플레이어가 닿으면 PrototypeStageManager를 통해 가장 오래된 방을 제거할 수 있습니다.
/// 삭제 가능 여부(canRemoveRoom)를 설정할 수 있습니다.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
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

    [Tooltip("플레이어가 이 EndPoint에 도달했을 때 방을 제거할지 여부입니다.")]
    public bool canRemoveRoom = true;
    public bool isFirstEndPoint = false;

    private bool triggered = false;

    private void Awake()
    {
        var collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player") && canRemoveRoom)
        {
            triggered = true;

            PrototypeStageManager.Instance.RemoveOldestRoom(); // 항상 가장 오래된 방 제거

            PrototypeStageManager.Instance.OnRoomCleared();    // 방 클리어 체크
        }
    }

    /// <summary>
    /// 선택된 방향을 Vector3로 변환하여 반환합니다.
    /// </summary>
    public Vector3 GetManualForward()
    {
        switch (direction)
        {
            case Direction.Forward:
                return Vector3.forward;
            case Direction.Backward:
                return Vector3.back;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            case Direction.Up:
                return Vector3.up;
            case Direction.Down:
                return Vector3.down;
            default:
                return Vector3.forward;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 dir = GetManualForward();
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + dir * 3.0f);
    }
#endif
}
