using UnityEngine;
using System.Collections.Generic;

#region Main Class

/// <summary>
/// Room 데이터를 기반으로 룸 프리팹을 로드하고 제어하는 클래스
/// </summary>
public class RoomLoader : MonoBehaviour
{
    #region Fields

    [Header("JSON 데이터 (TextAsset)")]
    [SerializeField] private TextAsset roomJson;

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject playerObject;

    private List<RoomData> roomDataList;
    private GameObject currentRoom;
    private GameObject previousRoom;
    private GameObject oldestRoom; // 가장 오래된 룸 추적
    private int currentRoomIndex = 0;

    private bool playerSpawned = false; // 최초 1회만 스폰 포인트 적용

    #endregion

    #region Unity Methods

    private void Awake()
    {
        var parsed = JsonUtility.FromJson<RoomDataList>(roomJson.text);
        roomDataList = parsed.Data;
    }

    private void Start()
    {
        LoadRoom(0);
    }

    #endregion

    #region Room Management

    /// <summary>
    /// 지정 인덱스의 룸을 로드
    /// </summary>
    /// <param name="index">로드할 룸 인덱스</param>
    public void LoadRoom(int index)
    {
        if (index >= roomDataList.Count)
        {
            Debug.LogWarning("[RoomLoader] 더 이상 로드할 룸이 없습니다.");
            return;
        }

        var data = roomDataList[index];

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Stage/Rooms/{data.RoomID}");
        if (prefab == null)
        {
            Debug.LogError($"[RoomLoader] {data.RoomID} 프리팹을 찾을 수 없습니다.");
            return;
        }

        // 룸 추적 업데이트
        oldestRoom = previousRoom;
        previousRoom = currentRoom;

        Vector3 position = ToVector3(data.RoomPos);
        Quaternion rotation = Quaternion.Euler(ToVector3(data.RoomRot));

        currentRoom = Instantiate(prefab, position, rotation);
        currentRoomIndex = index;

        if (!playerSpawned)
        {
            SpawnPlayer(currentRoom);
            playerSpawned = true;
        }

        Debug.Log($"[RoomLoader] Room {data.RoomID} 로드 완료");
    }

    /// <summary>
    /// 다음 룸 로드
    /// </summary>
    public void LoadNextRoom()
    {
        LoadRoom(currentRoomIndex + 1);
    }

    /// <summary>
    /// 두 단계 전 룸 제거
    /// </summary>
    public void RemovePrevRoom()
    {
        if (oldestRoom != null)
        {
            Destroy(oldestRoom);
            oldestRoom = null;
            Debug.Log("[RoomLoader] 가장 오래된 룸 제거 완료");
        }
    }

    #endregion

    #region Helpers

    /// <summary>
    /// 테스트용 스폰포인트
    /// </summary>
    /// <param name="room"></param>
    private void SpawnPlayer(GameObject room)
    {
        if (playerObject == null) return;

        Transform spawn = room.transform.Find("SpawnPoint");
        if (spawn == null)
        {
            Debug.LogWarning("[RoomLoader] SpawnPoint를 찾을 수 없습니다.");
            return;
        }

        CharacterController cc = playerObject.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        playerObject.transform.position = spawn.position;
        playerObject.transform.rotation = spawn.rotation;

        if (cc != null) cc.enabled = true;

        Debug.Log("[RoomLoader] 플레이어가 스폰되었습니다.");
    }

    private Vector3 ToVector3(float[] arr)
    {
        if (arr == null || arr.Length < 3) return Vector3.zero;
        return new Vector3(arr[0], arr[1], arr[2]);
    }

    #endregion
}

#endregion
