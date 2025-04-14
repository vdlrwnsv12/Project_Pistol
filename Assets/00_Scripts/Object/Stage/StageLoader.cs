using UnityEngine;

public class StageLoader : MonoBehaviour
{
    [Header("스테이지 프리팹 리스트")]
    [SerializeField] private GameObject[] stagePrefabs;

    [Header("다음 스테이지 위치")]
    [SerializeField] private Transform nextPoint; // 문 앞에 배치될 위치

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject playerObject;

    private GameObject spawnedPlayer;

    private GameObject previousStage;
    private GameObject currentStage;
    private int currentStageIndex = 0;

    private void Awake()
    {
        // nextPoint가 비어 있으면 자동으로 "NextPoint"를 찾음
        if (nextPoint == null)
        {
            GameObject found = GameObject.Find("NextPoint");
            if (found != null)
            {
                nextPoint = found.transform;
                Debug.Log("[StageLoader] nextPoint 자동 연결됨: " + nextPoint.name);
            }
            else
            {
                Debug.LogWarning("[StageLoader] nextPoint가 설정되지 않았고 자동으로도 찾을 수 없습니다.");
            }
        }
    }

    private void Start()
    {
        LoadInitialStage();
    }

    /// <summary>
    /// 시작 시 첫 스테이지 로드 및 플레이어 스폰
    /// </summary>
    private void LoadInitialStage()
    {
        if (stagePrefabs.Length == 0)
        {
            Debug.LogWarning("Stage Prefabs가 비어 있습니다.");
            return;
        }

        // 1스테이지 로드
        currentStage = Instantiate(stagePrefabs[0], Vector3.zero, Quaternion.identity);
        currentStageIndex = 0;

        Debug.Log("Stage 1 로드 완료");

        // SpawnPoint 찾기
        Transform spawnPoint = currentStage.transform.Find("SpawnPoint");

        if (spawnPoint == null)
        {
            Debug.LogWarning("SpawnPoint를 Stage 1에서 찾을 수 없습니다.");
            return;
        }

        // 💡 이미 있는 플레이어를 위치로 이동
        if (playerObject != null)
        {
            CharacterController cc = playerObject.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // 이동 전에 꺼줘야 위치 덮어쓰기 가능
            }

            playerObject.transform.position = spawnPoint.position;
            playerObject.transform.rotation = spawnPoint.rotation;

            if (cc != null)
            {
                cc.enabled = true;
            }

            Debug.Log("씬에 있는 플레이어가 SpawnPoint에 배치되었습니다.");
        }
        else
        {
            Debug.LogWarning("playerObject가 할당되지 않았습니다.");
        }
    }


    /// <summary>
    /// 다음 스테이지 로드 (NextPoint 위치 기준)
    /// </summary>
    public void LoadNextStage()
    {
        int nextIndex = currentStageIndex + 1;
        if (nextIndex >= stagePrefabs.Length)
        {
            Debug.Log("모든 스테이지 완료!");
            return;
        }

        previousStage = currentStage;

        //1. 프리팹 인스턴스 생성
        GameObject newStage = Instantiate(stagePrefabs[nextIndex], Vector3.zero, Quaternion.identity);
        currentStage = newStage;
        currentStageIndex = nextIndex;

        // 2. 프리팹 내부에서 NextPoint 직접 탐색
        Transform foundNextPoint = newStage.transform.Find("NextPoint");
        if (foundNextPoint != null)
        {
            nextPoint = foundNextPoint;
            Debug.Log($"[StageLoader] NextPoint 자동 연결 완료: {nextPoint.position}");
        }
        else
        {
            Debug.LogWarning("[StageLoader] NextPoint를 새로 생성된 스테이지에서 찾을 수 없습니다.");
        }

        // 3. 새로운 스테이지는 이전 NextPoint 기준으로 배치
        currentStage.transform.position = nextPoint != null ? nextPoint.position : Vector3.zero;

        Debug.Log($"Stage {currentStageIndex + 1} 로드 완료");
    }

    /// <summary>
    /// 이전 스테이지 제거
    /// </summary>
    public void RemovePreviousStage()
    {
        if (previousStage != null)
        {
            Destroy(previousStage);
            previousStage = null;

            Debug.Log("이전 스테이지 제거됨");
        }
    }
}
