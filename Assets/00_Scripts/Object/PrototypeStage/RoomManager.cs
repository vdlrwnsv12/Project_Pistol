using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private RoomSO[] rooms;
    
    private StandbyRoom standbyRoom;
    private Dictionary<string, ShootingRoom> shootingRoomDict;

    private void Awake()
    {
        rooms = ResourceManager.Instance.LoadAll<RoomSO>("Data/SO/RoomSO");
        
        var standbyRoomPrefab = ResourceManager.Instance.Load<StandbyRoom>("Prefabs/Prototype/StandbyRoom");
        standbyRoom = Instantiate(standbyRoomPrefab);
        standbyRoom.gameObject.SetActive(false);
        
        var roomPrefabs = ResourceManager.Instance.LoadAll<ShootingRoom>("Prefabs/Prototype/Room");
        shootingRoomDict = new Dictionary<string, ShootingRoom>();
        foreach (var room in roomPrefabs)
        {
            var shootingRoom = Instantiate(room);
            shootingRoom.gameObject.SetActive(false);
            shootingRoomDict.Add(room.name, shootingRoom);
        }
    }

    private void Start()
    {
        Transform tmp = transform;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        CreateStage(tmp);
    }

    private void CreateStage(Transform lastEndPoint)
    {
        var offsetRot = Quaternion.Inverse(standbyRoom.transform.rotation) * standbyRoom.StartPoint.rotation;
        standbyRoom.transform.rotation = lastEndPoint.rotation * Quaternion.Inverse(offsetRot);
        var offsetPos = standbyRoom.StartPoint.position - standbyRoom.transform.position;
        standbyRoom.transform.position = lastEndPoint.position - offsetPos;
        standbyRoom.gameObject.SetActive(true);
        
        offsetRot= Quaternion.Inverse(shootingRoomDict["R0001"].transform.rotation) * shootingRoomDict["R0001"].StartPoint.rotation;
        shootingRoomDict["R0001"].transform.rotation = standbyRoom.transform.rotation * Quaternion.Inverse(offsetRot);
        offsetPos = shootingRoomDict["R0001"].StartPoint.position - shootingRoomDict["R0001"].transform.position;
        shootingRoomDict["R0001"].transform.position = standbyRoom.EndPoint.transform.position - offsetPos;
        shootingRoomDict["R0001"].gameObject.SetActive(true);
        
        offsetRot= Quaternion.Inverse(shootingRoomDict["R0002"].transform.rotation) * shootingRoomDict["R0002"].StartPoint.rotation;
        shootingRoomDict["R0002"].transform.rotation = shootingRoomDict["R0001"].transform.rotation * Quaternion.Inverse(offsetRot);
        offsetPos = shootingRoomDict["R0002"].StartPoint.position - shootingRoomDict["R0002"].transform.position;
        shootingRoomDict["R0002"].transform.position = shootingRoomDict["R0001"].EndPoint.transform.position - offsetPos;
        shootingRoomDict["R0002"].gameObject.SetActive(true);
    }
}
