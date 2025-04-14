using UnityEngine;

/// <summary>
/// 기준 스테이지와 추가 구성물을 조합해 스테이지를 구성하는 매니저
/// </summary>
public class StageLoader : MonoBehaviour
{
    #region Fields

    [Header("기준 스테이지")]
    [SerializeField] private GameObject baseStagePrefab;

    [Header("추가 스테이지 구성물")]
    [SerializeField] private GameObject[] additiveStagePrefabs;

    private GameObject baseStageInstance;
    private GameObject currentAdditive;

    private int currentStageIndex;

    #endregion

    #region Stage Flow

    /// <summary>
    /// 시작시 스테이지 소환
    /// </summary>
    private void Start()
    {
        LoadFirstStage(); // 씬 시작 시 자동으로 첫 스테이지 배치
    }


    /// <summary>
    /// 첫 스테이지 로드
    /// </summary>
    public void LoadFirstStage()
    {
        currentStageIndex = 0;
        LoadStage(currentStageIndex);
    }

    /// <summary>
    /// 다음 스테이지 로드
    /// </summary>
    public void LoadNextStage()
    {
        currentStageIndex++;

        if (currentStageIndex >= additiveStagePrefabs.Length)
        {
            Debug.Log("모든 스테이지 완료");
            return;
        }

        LoadStage(currentStageIndex);
    }

    /// <summary>
    /// 스테이지 로드: 기준 스테이지 + 구성물
    /// </summary>
    /// <param name="index">스테이지 번호</param>
    private void LoadStage(int index)
    {
        // 기준 스테이지 생성 또는 유지
        if (baseStageInstance == null)
        {
            baseStageInstance = Instantiate(baseStagePrefab);
        }

        // 기존 구성물 제거
        if (currentAdditive != null)
        {
            Destroy(currentAdditive);
        }

        // 기준 바닥 위치 얻기
        BaseStageBehavior baseInfo = baseStageInstance.GetComponent<BaseStageBehavior>();
        Vector3 basePosition = baseInfo.GetFloorPosition();

        // 추가 구성물 생성
        currentAdditive = Instantiate(additiveStagePrefabs[index]);
        currentAdditive.transform.position = basePosition;

        Debug.Log($"Stage {index + 1} 로드 완료");
    }

    #endregion
}
