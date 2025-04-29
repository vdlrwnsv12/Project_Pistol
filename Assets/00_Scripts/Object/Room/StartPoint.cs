using UnityEngine;

/// <summary>
/// 방의 시작 지점입니다.  
/// 플레이어가 닿으면 다음 방을 생성하며, 씬에 Z+ 방향 표시를 그립니다.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class StartPoint : MonoBehaviour
{
    private bool triggered = false;

    private void Awake()
    {
        var collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            PrototypeStageManager.Instance.SpawnNextRoom();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
    }
#endif
}
