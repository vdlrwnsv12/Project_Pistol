using DataDeclaration;

public class StandbyRoom : Room
{
    private void Awake()
    {
        if (endPoint == null)
        {
            endPoint = transform.FindDeepChildByName("EndPoint");
        }
        
        exitGate.Door.OpenDoor += OpenDoor;
        exitGate.Door.DoorClosed += ResetRoom;
        exitGate.OnPassingGate += ExitRoom;
        enterGate.OnPassingGate += EnterRoom;
    }
    
    protected override void OpenDoor()
    {
        StageManager.Instance.IsGamePause = false;
        StageManager.Instance.RemainTime += Constants.ADDITIONAL_STAGE_TIME;
        
        RoomManager.Instance.PlaceNextRoom();
    }

    protected override void EnterRoom()
    {
        RoomManager.Instance.CurRoom = this;
        exitGate.Door.gameObject.SetActive(true);
        RoomManager.Instance.RoomChangedAction();
        UIManager.Instance.OpenPopupUI<PopupReward>();
        StageManager.Instance.IsGamePause = true;
    }

    protected override void ExitRoom()
    {
        RoomManager.Instance.PrevRoom = this;
        exitGate.Door.Close();
    }

    public override void ResetRoom()
    {
        exitGate.Door.Close();
        enterGate.Door.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}