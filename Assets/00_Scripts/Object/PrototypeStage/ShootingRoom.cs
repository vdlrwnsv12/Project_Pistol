using System.Collections.Generic;
using System.Linq;
using DataDeclaration;
using UnityEngine;

public class ShootingRoom : Room
{
    [SerializeField] private Transform[] targetPoints;
    private Transform[] activeWallPoints;

    private List<BaseTarget> targetList;
    
    public RoomSO Data { get; set; }

    protected override void Awake()
    {
        base.Awake();
        targetList = new List<BaseTarget>();
        RespawnTarget();
        exitDoor.OpenDoorAction += DoorTrigger;
    }

    private void OnEnable()
    {
        if (Data != null)
        {
            InitTarget();
        }
    }

    private void OnDisable()
    {
        foreach (var target in targetList)
        {
            target.gameObject.SetActive(false);
        }
    }

    private void DoorTrigger()
    {
        // StageManager.Instance.IsGamePause = false;
        // StageManager.Instance.RemainTime += Constants.ADDITIONAL_STAGE_TIME;
        //     
        // StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
        // StageManager.Instance.roomCreator.CurRoom = this;
        //     
        // StageManager.Instance.roomCreator.RoomChangedAction();
        //     
        // if (StageManager.Instance.roomCreator.CurRoomIndex == Constants.MAX_ROOM_INDEX)
        // {
        //     StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceStandbyRoom(endPoint);
        // }
        // else
        // {
        //     StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
        // }
        
        if (StageManager.Instance.roomCreator.CurStageIndex == Constants.LAST_STAGE_INDEX &&
            StageManager.Instance.roomCreator.CurRoomIndex == Constants.MAX_ROOM_INDEX)
        {
            StageManager.Instance.GameOver();
            return;
        }
        OpenRewardUI();
            
        StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
        StageManager.Instance.roomCreator.CurRoom = this;
            
        StageManager.Instance.roomCreator.RoomChangedAction();
        StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
    }
    
    private void OpenRewardUI()
    {
        UIManager.Instance.OpenPopupUI<PopupReward>();
        UIManager.ToggleMouseCursor(true);
        StageManager.Instance.IsGamePause = true;
        StageManager.Instance.Player.Controller.enabled = false;
    }

    private void InitTarget()
    {
        var targetIDList = Data.Targets;
        var activeTarget = GetRandomActiveTargetPoint();
        
        for (var i = 0; i < targetIDList.Length; i++)
        {
            var data = ResourceManager.Instance.Load<TargetSO>($"Data/SO/TargetSO/{targetIDList[i]}");
            targetList[i].InitData(data);
            targetList[i].gameObject.SetActive(true);
        }
    }

    private BaseTarget[] GetRandomActiveTargetPoint()
    {
        return targetList.OrderBy(o => Random.value).Take(Data.Targets.Length).ToArray();
    }

    private void RespawnTarget()
    {
        for (var i = 0; i < targetPoints.Length; i++)
        {
            var targetResource = ResourceManager.Instance.Load<BaseTarget>("Prefabs/Target/LandTarget");
            var target = Instantiate(targetResource, targetPoints[i]);
            target.gameObject.SetActive(false);
            targetList.Add(target);
        }
    }
}
