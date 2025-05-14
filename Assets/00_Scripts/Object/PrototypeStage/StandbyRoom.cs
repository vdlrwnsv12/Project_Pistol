using DataDeclaration;
using UnityEngine;

public class StandbyRoom : Room
{
    protected override void OpenDoor()
    {
        StageManager.Instance.IsGamePause = false;
        StageManager.Instance.RemainTime += Constants.ADDITIONAL_STAGE_TIME;
        
        RoomManager.Instance.PlaceNextRoom();
    }

    protected override void EnterRoom()
    {
    }

    protected override void ExitRoom()
    {
        RoomManager.Instance.PlaceNextRoom();

        RoomManager.Instance.RoomChangedAction();
    }
}