using System;
using System.Collections.Generic;
using System.Linq;
using DataDeclaration;
using Random = UnityEngine.Random;

public class RoomManager : SingletonBehaviour<RoomManager>
{
    private Dictionary<int, List<RoomSO>> allStageDataDict;

    private StandbyRoom standbyRoom;
    private Dictionary<int, ShootingRoom> shootingRoomDict;

    public int CurStageIndex { get; private set; }
    public int CurRoomIndex { get; private set; }

    public Room PrevRoom { get; set; }
    public Room CurRoom { get; set; }
    public Room NextRoom { get; set; }

    public Action RoomChangedAction;

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        InitStageInfo();
        InitRoomPrefabs();

        RoomChangedAction += UpdateStageIndex;
    }

    public void InitRoom()
    {
        var startRoom = ResourceManager.Instance.Load<StartRoom>("Prefabs/Stage/Room/StartRoom");
        CurRoom = Instantiate(startRoom);
    }

    public void PlaceNextRoom()
    {
        if (CurRoomIndex == 3)
        {
            NextRoom = standbyRoom;
        }
        else
        {
            var shootingRoomNumber = GetRoomNumber(allStageDataDict[CurStageIndex][CurRoomIndex].ID);
            NextRoom = shootingRoomDict[shootingRoomNumber];
        }

        NextRoom.transform.rotation = CurRoom.EndPoint.rotation;
        NextRoom.transform.position = CurRoom.EndPoint.position;
        NextRoom.gameObject.SetActive(true);
    }

    private void UpdateStageIndex()
    {
        if (CurRoomIndex == Constants.MAX_ROOM_INDEX)
        {
            CurRoomIndex = 0;
            CurStageIndex++;
        }
        else
        {
            CurRoomIndex++;
        }
    }

    private void InitStageInfo()
    {
        var roomDataList = ResourceManager.Instance.LoadAll<RoomSO>("Data/SO/RoomSO");
        allStageDataDict = new Dictionary<int, List<RoomSO>>();
        
        foreach (var roomData in roomDataList)
        {
            if (!allStageDataDict.ContainsKey(GetStageIndex(roomData.ID)))
            {
                var roomList = new List<RoomSO>();
                allStageDataDict.Add(GetStageIndex(roomData.ID), roomList);
            }

            allStageDataDict[GetStageIndex(roomData.ID)].Add(roomData);
        }

        foreach (var key in allStageDataDict.Keys.ToList())
        {
            var fullList = allStageDataDict[key];
            var random3Rooms = fullList.OrderBy(x => Random.value).Take(3).ToList();
            allStageDataDict[key] = random3Rooms;
        }

        CurStageIndex = 1;
        CurRoomIndex = 0;
    }

    private void InitRoomPrefabs()
    {
        var standbyRoomPrefab = ResourceManager.Instance.Load<StandbyRoom>("Prefabs/Stage/Room/StandbyRoom");
        standbyRoom = Instantiate(standbyRoomPrefab);
        standbyRoom.gameObject.SetActive(false);

        var roomPrefabs = ResourceManager.Instance.LoadAll<ShootingRoom>("Prefabs/Stage/Room");
        shootingRoomDict = new Dictionary<int, ShootingRoom>();
        foreach (var room in roomPrefabs)
        {
            var shootingRoom = Instantiate(room);
            shootingRoom.gameObject.SetActive(false);
            shootingRoomDict.Add(GetRoomNumber(room.name), shootingRoom);
        }
    }

    #region Utility

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

    #endregion
}