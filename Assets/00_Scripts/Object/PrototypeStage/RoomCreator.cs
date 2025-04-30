using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    private Dictionary<int, List<RoomSO>> stageDataDict;

    private StandbyRoom standbyRoom;
    private Dictionary<int, ShootingRoom> shootingRoomDict;

    private RoomSO[] stageRoomList;

    public int CurStageIndex { get; private set; }
    public int CurRoomIndex { get; private set; }

    public Room PrevRoom { get; set; }
    public Room CurRoom { get; set; }
    public Room NextRoom { get; set; }

    public StandbyRoom StandbyRoom => standbyRoom;

    private void Awake()
    {
        var roomDataList = ResourceManager.Instance.LoadAll<RoomSO>("Data/SO/RoomSO");
        stageDataDict = new Dictionary<int, List<RoomSO>>();
        foreach (var roomData in roomDataList)
        {
            if (!stageDataDict.ContainsKey(GetStageIndex(roomData.ID)))
            {
                var roomList = new List<RoomSO>();
                stageDataDict.Add(GetStageIndex(roomData.ID), roomList);
            }

            stageDataDict[GetStageIndex(roomData.ID)].Add(roomData);
        }

        var standbyRoomPrefab = ResourceManager.Instance.Load<StandbyRoom>("Prefabs/Prototype/StandbyRoom");
        standbyRoom = Instantiate(standbyRoomPrefab);
        standbyRoom.gameObject.SetActive(false);

        var roomPrefabs = ResourceManager.Instance.LoadAll<ShootingRoom>("Prefabs/Prototype/Room");
        shootingRoomDict = new Dictionary<int, ShootingRoom>();
        foreach (var room in roomPrefabs)
        {
            var shootingRoom = Instantiate(room);
            shootingRoom.gameObject.SetActive(false);
            shootingRoomDict.Add(GetRoomNumber(room.name), shootingRoom);
        }
        
        CurStageIndex = 1;
        CurRoomIndex = 0;
        stageRoomList = GetRandomRoomArray(CurStageIndex);
    }

    public Room PlaceStandbyRoom(Transform curRoomEndPoint)
    {
        var offsetRot = Quaternion.Inverse(standbyRoom.transform.rotation) * standbyRoom.StartPoint.rotation;
        standbyRoom.transform.rotation = curRoomEndPoint.rotation * Quaternion.Inverse(offsetRot);
        var offsetPos = standbyRoom.StartPoint.position - standbyRoom.transform.position;
        standbyRoom.transform.position = curRoomEndPoint.position - offsetPos;
        standbyRoom.gameObject.SetActive(true);
        return standbyRoom;
    }

    public Room PlaceShootingRoom(Transform curRoomEndPoint, int shootingRoomIndex)
    {
        var shootingRoom = shootingRoomDict[GetRoomNumber(stageRoomList[shootingRoomIndex].ID)];

        var offsetRot = Quaternion.Inverse(shootingRoom.transform.rotation) *
                        shootingRoom.StartPoint.rotation;
        shootingRoom.transform.rotation =
            curRoomEndPoint.transform.rotation * Quaternion.Inverse(offsetRot);
        var offsetPos = shootingRoom.StartPoint.position - shootingRoom.transform.position;
        shootingRoom.transform.position = curRoomEndPoint.transform.position - offsetPos;
        shootingRoom.Data = stageRoomList[shootingRoomIndex];
        shootingRoom.gameObject.SetActive(true);
        return shootingRoom;
    }

    public void DisablePrevRoom()
    {
        PrevRoom.gameObject.SetActive(false);
    }

    public void UpdateStageIndex()
    {
        if (CurRoomIndex == 3)
        {
            CurRoomIndex = 0;
            CurStageIndex++;
            stageRoomList = GetRandomRoomArray(CurStageIndex);
        }
        else
        {
            CurRoomIndex++;
        }
        StageManager.Instance.HUDUI.UpdateStageInfo(CurStageIndex, CurRoomIndex);
    }

    private int GetStageIndex(string roomID)
    {
        var numericId = int.Parse(roomID.Substring(1));
        var stageIndex = numericId / 1000;

        return stageIndex;
    }

    private int GetRoomNumber(string roomID)
    {
        var numericId = int.Parse(roomID.Substring(1));
        var roomNumber = numericId % 1000;

        return roomNumber;
    }

    private RoomSO[] GetRandomRoomArray(int stageIndex)
    {
        return stageDataDict[stageIndex].OrderBy(o => Random.value).Take(3).ToArray();
    }
}