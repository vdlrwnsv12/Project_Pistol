using DataDeclaration;
using UnityEngine;

public class StandbyRoom : Room
{
    public Transform RespawnPoint { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        RespawnPoint = transform.FindDeepChildByName("RespawnPoint");
        exitDoor.OpenDoorAction += DoorTrigger;
    }

    private void OpenRewardUI()
    {
        UIManager.Instance.OpenPopupUI<PopupReward>();
        UIManager.ToggleMouseCursor(true);
        StageManager.Instance.IsGamePause = true;
        StageManager.Instance.Player.Controller.enabled = false;
    }

    private void DoorTrigger()
    {
        // if (StageManager.Instance.roomCreator.CurStageIndex == Constants.LAST_STAGE_INDEX &&
        //     StageManager.Instance.roomCreator.CurRoomIndex == Constants.MAX_ROOM_INDEX)
        // {
        //     StageManager.Instance.GameOver();
        //     return;
        // }
        // OpenRewardUI();
        //     
        // StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
        // StageManager.Instance.roomCreator.CurRoom = this;
        //     
        // StageManager.Instance.roomCreator.RoomChangedAction();
        // StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
        
        
        StageManager.Instance.IsGamePause = false;
        StageManager.Instance.RemainTime += Constants.ADDITIONAL_STAGE_TIME;
            
        StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
        StageManager.Instance.roomCreator.CurRoom = this;
            
        StageManager.Instance.roomCreator.RoomChangedAction();
            
        if (StageManager.Instance.roomCreator.CurRoomIndex == Constants.MAX_ROOM_INDEX)
        {
            StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceStandbyRoom(endPoint);
        }
        else
        {
            StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
        }
    }
}