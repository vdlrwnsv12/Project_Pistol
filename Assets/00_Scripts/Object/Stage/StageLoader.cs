using UnityEngine;
using System.Collections.Generic;

#region Main Class

/// <summary>
/// Stage 데이터를 기반으로 스테이지 프리팹을 로드하고 제어하는 클래스
/// </summary>
public class StageLoader : MonoBehaviour
{
    #region Fields

    [Header("JSON 데이터 (TextAsset)")]
    [SerializeField] private TextAsset stageJson;

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject playerObject;

    private List<StageData> stageDataList;
    private GameObject currentStage;
    private GameObject previousStage;
    private GameObject oldestStage; // 가장 오래된 스테이지 추적
    private int currentStageIndex = 0;

    private bool playerSpawned = false; // 최초 1회만 스폰 포인트 적용

    #endregion

    #region Unity Methods

    private void Awake()
    {
        var parsed = JsonUtility.FromJson<StageDataList>(stageJson.text);
        stageDataList = parsed.Data;
    }

    private void Start()
    {
        LoadStage(0);
    }

    #endregion

    #region Stage Management

    /// <summary>
    /// 지정 인덱스의 스테이지 로드
    /// </summary>
    /// <param name="index">로드할 스테이지 인덱스</param>
    public void LoadStage(int index)
    {
        if (index >= stageDataList.Count)
        {
            Debug.LogWarning("[StageLoader] 더 이상 로드할 스테이지가 없습니다.");
            return;
        }

        var data = stageDataList[index];

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Stage/Rooms/{data.RoomID}");
        if (prefab == null)
        {
            Debug.LogError($"[StageLoader] {data.RoomID} 프리팹을 찾을 수 없습니다.");
            return;
        }

        // 스테이지 추적 업데이트
        oldestStage = previousStage;
        previousStage = currentStage;

        Vector3 position = ToVector3(data.RoomPos);
        Quaternion rotation = Quaternion.Euler(ToVector3(data.RoomRot));

        currentStage = Instantiate(prefab, position, rotation);
        currentStageIndex = index;

        // SpawnPoint는 최초 한 번만 적용
        if (!playerSpawned)
        {
            SpawnPlayer(currentStage);
            playerSpawned = true;
        }

        Debug.Log($"[StageLoader] Stage {data.ID} 로드 완료");
    }

    /// <summary>
    /// 다음 스테이지 로드
    /// </summary>
    public void LoadNextStage()
    {
        LoadStage(currentStageIndex + 1);
    }

    /// <summary>
    /// 두 단계 전 스테이지 제거
    /// </summary>
    public void RemovePreviousStage()
    {
        if (oldestStage != null)
        {
            Destroy(oldestStage);
            oldestStage = null;
            Debug.Log("[StageLoader] 가장 오래된 스테이지 제거 완료");
        }
    }

    #endregion

    #region Helpers

    /// <summary>
    /// 스테이지 내 SpawnPoint 위치로 플레이어 이동
    /// </summary>
    /// <param name="stage">현재 스테이지</param>
    private void SpawnPlayer(GameObject stage)
    {
        if (playerObject == null) return;

        Transform spawn = stage.transform.Find("SpawnPoint");
        if (spawn == null)
        {
            Debug.LogWarning("[StageLoader] SpawnPoint를 찾을 수 없습니다.");
            return;
        }

        CharacterController cc = playerObject.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        playerObject.transform.position = spawn.position;
        playerObject.transform.rotation = spawn.rotation;

        if (cc != null) cc.enabled = true;

        Debug.Log("[StageLoader] 플레이어가 스폰되었습니다.");
    }

    /// <summary>
    /// float 배열을 Vector3로 변환
    /// </summary>
    /// <param name="arr">[x, y, z] 배열</param>
    /// <returns>Vector3로 변환된 값</returns>
    private Vector3 ToVector3(float[] arr)
    {
        if (arr.Length < 3) return Vector3.zero;
        return new Vector3(arr[0], arr[1], arr[2]);
    }

    #endregion
}

#endregion

#region Data Classes

/// <summary>
/// 스테이지 데이터 리스트 (JSON 루트)
/// </summary>
[System.Serializable]
public class StageDataList
{
    public List<StageData> Data;
}

#endregion
