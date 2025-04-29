using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    private Dictionary<int, List<RoomSO>> stageDataDict;

    private StandbyRoom standbyRoom;
    private Dictionary<int, ShootingRoom> shootingRoomDict;
    
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
    }

    public void CreateStage(Transform lastEndPoint, int stageIndex)
    {
        StageManager.Instance.CurStageIndex = stageIndex;
        
        var roomList = GetRandomRoomArray(stageIndex);
        
        var offsetRot = Quaternion.Inverse(standbyRoom.transform.rotation) * standbyRoom.StartPoint.rotation;
        standbyRoom.transform.rotation = lastEndPoint.rotation * Quaternion.Inverse(offsetRot);
        var offsetPos = standbyRoom.StartPoint.position - standbyRoom.transform.position;
        standbyRoom.transform.position = lastEndPoint.position - offsetPos;
        standbyRoom.gameObject.SetActive(true);
        standbyRoom.RoomIndex = 0;

        offsetRot = Quaternion.Inverse(shootingRoomDict[GetRoomNumber(roomList[0].ID)].transform.rotation) *
                    shootingRoomDict[GetRoomNumber(roomList[0].ID)].StartPoint.rotation;
        shootingRoomDict[GetRoomNumber(roomList[0].ID)].transform.rotation =
            standbyRoom.EndPoint.transform.rotation * Quaternion.Inverse(offsetRot);
        offsetPos = shootingRoomDict[GetRoomNumber(roomList[0].ID)].StartPoint.position - shootingRoomDict[GetRoomNumber(roomList[0].ID)].transform.position;
        shootingRoomDict[GetRoomNumber(roomList[0].ID)].transform.position = standbyRoom.EndPoint.transform.position - offsetPos;
        shootingRoomDict[GetRoomNumber(roomList[0].ID)].Data = roomList[0];
        shootingRoomDict[GetRoomNumber(roomList[0].ID)].RoomIndex = 1;
        shootingRoomDict[GetRoomNumber(roomList[0].ID)].gameObject.SetActive(true);

        offsetRot = Quaternion.Inverse(shootingRoomDict[GetRoomNumber(roomList[1].ID)].transform.rotation) *
                    shootingRoomDict[GetRoomNumber(roomList[1].ID)].StartPoint.rotation;
        shootingRoomDict[GetRoomNumber(roomList[1].ID)].transform.rotation =
            shootingRoomDict[GetRoomNumber(roomList[0].ID)].EndPoint.transform.rotation * Quaternion.Inverse(offsetRot);
        offsetPos = shootingRoomDict[GetRoomNumber(roomList[1].ID)].StartPoint.position - shootingRoomDict[GetRoomNumber(roomList[1].ID)].transform.position;
        shootingRoomDict[GetRoomNumber(roomList[1].ID)].transform.position =
            shootingRoomDict[GetRoomNumber(roomList[0].ID)].EndPoint.transform.position - offsetPos;
        shootingRoomDict[GetRoomNumber(roomList[1].ID)].Data = roomList[1];
        shootingRoomDict[GetRoomNumber(roomList[1].ID)].RoomIndex = 2;
        shootingRoomDict[GetRoomNumber(roomList[1].ID)].gameObject.SetActive(true);
        
        offsetRot = Quaternion.Inverse(shootingRoomDict[GetRoomNumber(roomList[2].ID)].transform.rotation) *
                    shootingRoomDict[GetRoomNumber(roomList[2].ID)].StartPoint.rotation;
        shootingRoomDict[GetRoomNumber(roomList[2].ID)].transform.rotation =
            shootingRoomDict[GetRoomNumber(roomList[1].ID)].EndPoint.transform.rotation * Quaternion.Inverse(offsetRot);
        offsetPos = shootingRoomDict[GetRoomNumber(roomList[2].ID)].StartPoint.position - shootingRoomDict[GetRoomNumber(roomList[2].ID)].transform.position;
        shootingRoomDict[GetRoomNumber(roomList[2].ID)].transform.position =
            shootingRoomDict[GetRoomNumber(roomList[1].ID)].EndPoint.transform.position - offsetPos;
        shootingRoomDict[GetRoomNumber(roomList[2].ID)].Data = roomList[2];
        shootingRoomDict[GetRoomNumber(roomList[2].ID)].RoomIndex = 3;
        shootingRoomDict[GetRoomNumber(roomList[2].ID)].gameObject.SetActive(true);
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