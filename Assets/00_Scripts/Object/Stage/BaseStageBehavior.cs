using UnityEngine;

/// <summary>
/// 기준 스테이지에서 필요한 기본 정보를 제공하는 클래스
/// </summary>
public class BaseStageBehavior : MonoBehaviour
{
    [Header("기준 바닥")]
    [SerializeField] private Transform floor;

    /// <summary>
    /// 바닥의 중심 위치 반환
    /// </summary>
    public Vector3 GetFloorPosition()
    {
        return floor != null ? floor.position : transform.position;
    }

    /// <summary>
    /// 바닥의 스케일 반환
    /// </summary>
    public Vector3 GetFloorScale()
    {
        return floor != null ? floor.localScale : Vector3.one;
    }
}
