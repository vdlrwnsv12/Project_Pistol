using System.Collections.Generic;
using UnityEngine;

public class ShootingRoom : Room
{
    [SerializeField] private Transform[] targetPoints;
    private Transform[] activeWallPoints;
    
    public RoomSO Data { get; set; }

    private void OnEnable()
    {
        if (Data != null)
        {
            RespawnTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.IsGamePause = false;
            StageManager.Instance.RemainTime += 20f;
            
            StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
            StageManager.Instance.roomCreator.DisablePrevRoom();
            StageManager.Instance.roomCreator.CurRoom = this;
            
            StageManager.Instance.roomCreator.UpdateStageIndex();
            
            if (StageManager.Instance.roomCreator.CurRoomIndex == 3)
            {
                StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceStandbyRoom(endPoint);
            }
            else
            {
                StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
            }
        }
    }

    private void RespawnTarget()
    {
        var targetIDList = Data.Targets;
        
        for (var i = 0; i < targetIDList.Length; i++)
        {
            var targetResource = ResourceManager.Instance.Load<BaseTarget>("Prefabs/Target/LandTarget");
            var target = Instantiate(targetResource, targetPoints[i]);
            var data = ResourceManager.Instance.Load<TargetSO>($"Data/SO/TargetSO/{targetIDList[i]}");
            target.InitData(data);
        }
    }
}
