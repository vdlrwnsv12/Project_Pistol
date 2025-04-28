using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 스탠바이룸과 랜덤 3개 룸을 순차적으로 이어붙여 스테이지를 구성하는 매니저입니다.
/// 현재 출구 방향(currentForward)을 갱신하여 다음 방을 자연스럽게 연결합니다.
/// </summary>
public class PrototypeStageManager : MonoBehaviour
{
    #region Fields

    [Header("Stage Settings")]
    public GameObject standbyRoomPrefab;
    public List<GameObject> roomPrefabs; // R1001 ~ R1005 프리팹 리스트

    [Header("JSON Settings")]
    public TextAsset roomDataJson; // Room.json 파일

    private RoomJsonWrapper roomDataList;
    private List<RoomJsonData> currentStageRooms = new List<RoomJsonData>();

    private GameObject currentStandbyRoom;
    private List<GameObject> currentShootingRooms = new List<GameObject>();

    private int currentStageId = 1;
    private Vector3 currentForward = Vector3.forward; // 최초 방향: 앞으로(Z+)

    #endregion

    #region Unity Methods

    private void Start()
    {
        LoadRoomData();
        CreateStage();
    }

    #endregion

    #region Private Methods

    private void LoadRoomData()
    {
        if (roomDataJson == null)
        {
            Debug.LogError("RoomData JSON 파일이 연결되지 않았습니다.");
            return;
        }

        roomDataList = JsonUtility.FromJson<RoomJsonWrapper>(roomDataJson.text);
    }

    private void CreateStage()
    {
        if (roomDataList == null || roomDataList.Data == null)
        {
            Debug.LogError("RoomData를 불러오지 못했습니다.");
            return;
        }

        currentStageRooms.Clear();

        foreach (var room in roomDataList.Data)
        {
            if (room.ID.StartsWith($"R{currentStageId}"))
            {
                currentStageRooms.Add(room);
            }
        }

        if (currentStageRooms.Count < 3)
        {
            Debug.LogError($"Stage {currentStageId}에 사용할 Room이 부족합니다.");
            return;
        }

        // StandbyRoom 생성
        currentStandbyRoom = Instantiate(standbyRoomPrefab);
        currentStandbyRoom.transform.position = Vector3.zero;
        currentStandbyRoom.transform.rotation = Quaternion.identity;

        EndPoint lastEndPoint = currentStandbyRoom.GetComponentInChildren<EndPoint>();
        currentForward = lastEndPoint.GetManualForward(); // 초기 방향 업데이트

        // Room 3개 랜덤 선택
        List<RoomJsonData> selectedRooms = PickRandomRooms(currentStageRooms, 3);

        foreach (var roomData in selectedRooms)
        {
            GameObject roomPrefab = roomPrefabs.Find(x => x.name == roomData.ID);
            if (roomPrefab == null)
            {
                Debug.LogWarning($"{roomData.ID} 프리팹을 찾을 수 없습니다.");
                continue;
            }

            var roomInstance = Instantiate(roomPrefab);
            roomInstance.name = roomPrefab.name;

            StartPoint currStartPoint = roomInstance.GetComponentInChildren<StartPoint>();
            EndPoint currEndPoint = roomInstance.GetComponentInChildren<EndPoint>();

            if (lastEndPoint != null && currStartPoint != null)
            {
                AlignRooms(lastEndPoint, currStartPoint, roomInstance);
            }

            lastEndPoint = currEndPoint;
            currentForward = currEndPoint.GetManualForward(); // 다음 방향 업데이트
            currentShootingRooms.Add(roomInstance);
        }
    }

    /// <summary>
    /// 이전 EndPoint의 방향을 기준으로, Y축 회전만 적용하여 Room을 정렬합니다.
    /// </summary>
    private void AlignRooms(EndPoint previousEndPoint, StartPoint currentStartPoint, GameObject currentRoom)
    {
        if (previousEndPoint == null || currentStartPoint == null || currentRoom == null)
        {
            Debug.LogError("AlignRooms 실패: 포인트나 룸이 null입니다.");
            return;
        }

        // 1. 이전 EndPoint에서 목표 방향 가져오기
        Vector3 nextDirection = previousEndPoint.GetManualForward().normalized;

        // 2. currentForward(현재 바라보는 방향)와 nextDirection(다음 갈 방향) 비교
        float angle = Vector3.SignedAngle(Vector3.forward, currentForward, Vector3.up);

        // 3. 현재 Room을 currentForward 방향에 맞춰 미리 회전
        currentRoom.transform.Rotate(Vector3.up, angle);

        // 4. 위치 덮어쓰기 (StartPoint를 EndPoint 위치로 이동)
        Vector3 offset = previousEndPoint.transform.position - currentStartPoint.transform.position;
        currentRoom.transform.position += offset;
    }

    private List<RoomJsonData> PickRandomRooms(List<RoomJsonData> roomList, int count)
    {
        List<RoomJsonData> result = new List<RoomJsonData>();
        List<RoomJsonData> temp = new List<RoomJsonData>(roomList);

        for (int i = 0; i < count && temp.Count > 0; i++)
        {
            int index = Random.Range(0, temp.Count);
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return result;
    }

    public void ClearStage()
    {
        if (currentStandbyRoom != null)
        {
            Destroy(currentStandbyRoom);
        }

        foreach (var room in currentShootingRooms)
        {
            Destroy(room);
        }

        currentShootingRooms.Clear();
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
