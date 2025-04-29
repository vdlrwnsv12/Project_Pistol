using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 스탠바이룸과 룸을 순차적으로 생성하고,  
/// 스테이지 클리어를 자동 감지하는 매니저입니다.
/// </summary>
public class PrototypeStageManager : MonoBehaviour
{
    #region Fields

    [Header("Prefab Settings")]
    public GameObject standbyRoomPrefab;
    public List<GameObject> roomPrefabs;

    private List<GameObject> selectedRooms = new List<GameObject>();
    private Queue<GameObject> spawnedRooms = new Queue<GameObject>();

    private GameObject standbyRoomInstance;
    private EndPoint lastEndPoint;

    private int currentStageId = 1;
    private int roomSpawnIndex = 0;
    private int roomClearCount = 0;
    private Vector3 currentForward = Vector3.right;

    public static PrototypeStageManager Instance { get; private set; }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartStage();
    }

    #endregion

    #region Stage Flow

    private void StartStage()
    {
        selectedRooms = PickRandomRooms(roomPrefabs, 3);
        roomSpawnIndex = 0;
        roomClearCount = 0;
        spawnedRooms.Clear();

        SpawnStandbyRoom();
        SpawnNextRoom(); // 첫 번째 Room 소환
    }

    private void SpawnStandbyRoom()
    {
        standbyRoomInstance = Instantiate(standbyRoomPrefab);
        standbyRoomInstance.transform.position = Vector3.zero;
        standbyRoomInstance.transform.rotation = Quaternion.identity;

        lastEndPoint = standbyRoomInstance.GetComponentInChildren<EndPoint>();
        currentForward = lastEndPoint.GetManualForward();

        spawnedRooms.Enqueue(standbyRoomInstance);
    }

    /// <summary>
    /// 다음 방을 생성합니다.
    /// </summary>
    public void SpawnNextRoom()
    {
        if (roomSpawnIndex >= selectedRooms.Count)
        {
            Debug.Log("[Stage] 모든 방 생성 완료");
            return;
        }

        GameObject roomPrefab = selectedRooms[roomSpawnIndex];
        GameObject roomInstance = Instantiate(roomPrefab);

        StartPoint startPoint = roomInstance.GetComponentInChildren<StartPoint>();
        EndPoint endPoint = roomInstance.GetComponentInChildren<EndPoint>();

        if (roomSpawnIndex == 0 && endPoint != null)
        {
            endPoint.isFirstEndPoint = false; // 첫 번째 룸만 StandbyRoom 제거
        }

        AlignRooms(lastEndPoint, startPoint, roomInstance);

        lastEndPoint = endPoint;
        currentForward = endPoint.GetManualForward();

        spawnedRooms.Enqueue(roomInstance);
        roomSpawnIndex++;
    }

    public void RemoveStandbyRoom()
    {
        if (standbyRoomInstance != null)
        {
            Destroy(standbyRoomInstance);
            standbyRoomInstance = null;
            Debug.Log("[Remove] 스탠바이룸 제거 완료");
        }
    }

    public void RemoveOldestRoom()
    {
        if (spawnedRooms.Count > 0)
        {
            GameObject oldestRoom = spawnedRooms.Dequeue();
            if (oldestRoom != null)
            {
                Destroy(oldestRoom);

                // StandbyRoom 삭제 체크
                if (oldestRoom == standbyRoomInstance)
                {
                    standbyRoomInstance = null;
                    Debug.Log("[Remove] 스탠바이룸 제거 완료");
                }
                else
                {
                    Debug.Log($"[Remove] {oldestRoom.name} 제거 완료");
                }
            }
        }
    }

    /// <summary>
    /// 방을 클리어했을 때 호출합니다.
    /// </summary>
    public void OnRoomCleared()
    {
        roomClearCount++;

        if (roomClearCount >= selectedRooms.Count)
        {
            Debug.Log($"[Stage {currentStageId}] 스테이지 클리어!");

            StartNextStage(); // 바로 다음 스테이지 시작
        }
    }

    /// <summary>
    /// 다음 스테이지를 시작합니다.
    /// </summary>
    private void StartNextStage()
    {
        ClearCurrentStage(); // 현재 스테이지 정리
        currentStageId++;    // 스테이지 ID 증가

        selectedRooms = PickRandomRooms(roomPrefabs, 3); // 새로운 3개 룸 리스트 뽑기
        roomSpawnIndex = 0;
        roomClearCount = 0;
        spawnedRooms.Clear();

        SpawnStandbyRoom(); // 새 스탠바이룸 생성
        SpawnNextRoom();    // 새 첫 번째 룸 생성
    }


    private void ClearCurrentStage()
    {
        while (spawnedRooms.Count > 0)
        {
            var room = spawnedRooms.Dequeue();
            if (room != null)
            {
                Destroy(room);
            }
        }
    }

    private List<GameObject> PickRandomRooms(List<GameObject> pool, int count)
    {
        List<GameObject> result = new List<GameObject>();
        List<GameObject> temp = new List<GameObject>(pool);

        for (int i = 0; i < count && temp.Count > 0; i++)
        {
            int index = Random.Range(0, temp.Count);
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return result;
    }

    private void AlignRooms(EndPoint previousEndPoint, StartPoint currentStartPoint, GameObject currentRoom)
    {
        if (previousEndPoint == null || currentStartPoint == null || currentRoom == null)
        {
            Debug.LogError("AlignRooms 실패: 포인트나 룸이 null입니다.");
            return;
        }

        Vector3 nextDirection = previousEndPoint.GetManualForward().normalized;
        float angle = Vector3.SignedAngle(Vector3.forward, currentForward, Vector3.up);

        currentRoom.transform.Rotate(Vector3.up, angle);

        Vector3 offset = previousEndPoint.transform.position - currentStartPoint.transform.position;
        currentRoom.transform.position += offset;
    }

    #endregion
}

#region Sub Data Structures

[System.Serializable]
public class RoomJsonWrapper
{
    public List<RoomJsonData> Data;
}

[System.Serializable]
public class RoomJsonData
{
    public string ID;
    public List<string> Targets;
    public List<int> ActiveTargetPos;
    public List<int> ActiveWallPos;
}

#endregion
