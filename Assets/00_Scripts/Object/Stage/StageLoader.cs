using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stage 데이터를 기반으로 프리팹을 자동 로드하고 플레이어를 배치하는 스테이지 로더
/// </summary>
public class StageLoader : MonoBehaviour
{
    #region Fields

    [Header("스테이지 JSON 데이터")]
    [SerializeField] private TextAsset stageJsonFile;

    [Header("플레이어 오브젝트")]
    [SerializeField] private GameObject playerObject;

    private int currentStageIndex = 0;
    private GameObject currentStage;
    private GameObject previousStage;

    private StageDataList stageDataList;

    #endregion

    #region Unity

    private void Awake()
    {
        if (stageJsonFile == null)
        {
            Debug.LogError("[StageLoader] JSON 파일이 할당되지 않았습니다.");
            return;
        }

        stageDataList = JsonUtility.FromJson<StageDataList>(stageJsonFile.text);
    }

    private void Start()
    {
        LoadStage(0);
    }

    #endregion

    #region Stage Logic

    /// <summary>
    /// 특정 인덱스의 스테이지를 로드합니다.
    /// </summary>
    /// <param name="index">스테이지 인덱스</param>
    public void LoadStage(int index)
    {
        if (stageDataList == null || index >= stageDataList.Data.Count)
        {
            Debug.LogWarning("[StageLoader] 스테이지 데이터가 비었거나 범위를 초과했습니다.");
            return;
        }

        StageData data = stageDataList.Data[index];

        // RoomID로 프리팹 자동 로드 (Resources/Stage/Rooms/)
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Stage/Rooms/{data.RoomID}");
        if (prefab == null)
        {
            Debug.LogError($"[StageLoader] 01_Resources/Resources/Prefabs/Stage/Rooms/{data.RoomID}.prefab 을 찾을 수 없습니다.");
            return;
        }

        Vector3 position = ToVector3(data.RoomPos);
        Quaternion rotation = Quaternion.Euler(ToVector3(data.RoomRot));

        // 이전 스테이지 저장 후 제거
        previousStage = currentStage;
        currentStage = Instantiate(prefab, position, rotation);
        currentStageIndex = index;

        // 플레이어 스폰 처리
        SpawnPlayerToPoint(currentStage);

        Debug.Log($"[StageLoader] {data.ID} 스테이지 로드 완료 at {position}");
    }

    /// <summary>
    /// 다음 스테이지 로드
    /// </summary>
    public void LoadNextStage()
    {
        LoadStage(currentStageIndex + 1);
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
            Debug.Log("[StageLoader] 이전 스테이지 제거됨");
        }
    }

    #endregion

    #region Utilities

    /// <summary>
    /// float 배열을 Vector3로 변환
    /// </summary>
    private Vector3 ToVector3(float[] arr)
    {
        if (arr == null || arr.Length < 3) return Vector3.zero;
        return new Vector3(arr[0], arr[1], arr[2]);
    }

    /// <summary>
    /// 스폰 포인트에 플레이어 배치
    /// </summary>
    private void SpawnPlayerToPoint(GameObject stage)
    {
        if (playerObject == null)
        {
            Debug.LogWarning("[StageLoader] 플레이어 오브젝트가 없습니다.");
            return;
        }

        Transform spawnPoint = stage.transform.Find("SpawnPoint");
        if (spawnPoint == null)
        {
            Debug.LogWarning("[StageLoader] SpawnPoint를 찾을 수 없습니다.");
            return;
        }

        CharacterController cc = playerObject.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        playerObject.transform.position = spawnPoint.position;
        playerObject.transform.rotation = spawnPoint.rotation;

        if (cc != null) cc.enabled = true;

        Debug.Log("[StageLoader] 플레이어가 SpawnPoint에 배치되었습니다.");
    }

    #endregion
}

#region Data Classes

/// <summary>
/// JSON 루트 데이터
/// </summary>
[System.Serializable]
public class StageDataList
{
    public List<StageData> Data;
}

#endregion