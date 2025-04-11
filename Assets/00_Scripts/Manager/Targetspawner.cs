using UnityEngine;

#region TargetSpawner : 타겟 생성 제어 (수동 배치 기반)
/// <summary>
/// 스테이지마다 위치를 수동으로 지정해서 타겟을 생성하는 스크립트
/// (자동 랜덤 배치가 아닌, 미리 지정된 위치에서 타겟을 생성함)
/// </summary>
public class TargetSpawner : MonoBehaviour
{
    #region Variables

    [Header("일반 타겟")]
    [SerializeField] private GameObject targetPrefab;             // 일반 타겟 프리팹
    [SerializeField] private Transform[] targetPoints;            // 일반 타겟이 생성될 위치들을 배열로 저장

    [Header("벽/오브젝트 타겟")]
    [SerializeField] private GameObject wallTargetPrefab;         // 벽에 부착될 특수 타겟 프리팹
    [SerializeField] private Transform[] wallTargetPoints;        // 벽 타겟이 생성될 위치들을 배열로 저장

    #endregion

    #region Public Methods

    /// <summary>
    /// 외부에서 이 메서드를 호출하면,
    /// 미리 설정해둔 위치에 일반 타겟과 벽 타겟을 모두 생성한다.
    /// </summary>
    public void SpawnTargets()
    {
        SpawnNormalTargets();     // 일반 타겟 생성
        SpawnWallTargets();       // 벽 타겟 생성
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// targetPoints 배열에 담긴 위치들에 일반 타겟 프리팹을 하나씩 생성한다.
    /// </summary>
    private void SpawnNormalTargets()
    {
        foreach (Transform point in targetPoints)
        {
            if (point != null && targetPrefab != null)
            {
                // 타겟 프리팹을 해당 위치에 생성 (회전도 포함)
                Instantiate(targetPrefab, point.position, point.rotation);
            }
        }
    }

    /// <summary>
    /// wallTargetPoints 배열에 담긴 위치들에 벽 타겟 프리팹을 하나씩 생성한다.
    /// 주로 벽에 붙어 있는 정적인 표적에 사용된다.
    /// </summary>
    private void SpawnWallTargets()
    {
        foreach (Transform point in wallTargetPoints)
        {
            if (point != null && wallTargetPrefab != null)
            {
                // 벽 타겟 프리팹을 해당 위치에 생성 (회전도 포함)
                Instantiate(wallTargetPrefab, point.position, point.rotation);
            }
        }
    }

    #endregion
}
#endregion
