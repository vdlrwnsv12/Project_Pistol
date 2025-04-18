using System.Linq;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    [Header("스테이지 프리팹 리스트")]
    [SerializeField] private GameObject[] stagePrefabs;

    [Header("다음 스테이지 위치 배열 (Vector3)")]
    [SerializeField] private Vector3[] stagePositions;

    [Header("다음 스테이지 회전 배열 (Euler Angles)")]
    [SerializeField] private Vector3[] stageRotations;

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject playerObject;

    private GameObject spawnedPlayer;

    private GameObject previousStage;
    private GameObject currentStage;
    private int currentStageIndex = 0;

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

        //이미 있는 플레이어를 위치로 이동
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

        StageData nextData = stageDatabase.GetStageByIndex(nextIndex);
        if (nextData == null)
        {
            Debug.Log("모든 스테이지 완료!");
            return;
        }

        previousStage = currentStage;

        GameObject prefab = stagePrefabs[nextIndex]; // or nextData.ID로 프리팹 매칭 가능
        Vector3 pos = nextData.RoomPos;
        Quaternion rot = Quaternion.Euler(nextData.RoomRot);

        currentStage = Instantiate(prefab, pos, rot);
        currentStageIndex = nextIndex;

        Debug.Log($"Stage {currentStageIndex + 1} 로드 완료 (위치: {pos}, 회전: {rot.eulerAngles})");
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
