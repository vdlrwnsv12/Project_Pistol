using DataDeclaration;

public class StandbyRoom : Room
{
    protected override void Awake()
    {
        base.Awake();
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
        base.OpenDoor();
        
        StageManager.Instance.IsGamePause = false;
        StageManager.Instance.RemainTime += Constants.ADDITIONAL_STAGE_TIME;
    }

    protected override void EnterRoom()
    {
        base.EnterRoom();
        
        exitGate.Door.gameObject.SetActive(true);
        RoomManager.Instance.RoomChangedAction();
        UIManager.Instance.OpenPopupUI<PopupReward>();
        StageManager.Instance.IsGamePause = true;
    }

    protected override void ExitRoom()
    {
        base.ExitRoom();
        exitGate.Door.Close();
    }

    public override void ResetRoom()
    {
        exitGate.Door.Close();
        enterGate.Door.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}