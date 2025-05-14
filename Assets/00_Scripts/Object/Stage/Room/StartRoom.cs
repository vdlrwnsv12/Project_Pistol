public class StartRoom : Room
{
    private void Awake()
    {
        if (endPoint == null)
        {
            endPoint = transform.FindDeepChildByName("EndPoint");
        }
        
        exitGate.Door.OpenDoor += OpenDoor;
        exitGate.OnPassingGate += ExitRoom;
        
        EnterRoom();
    }
    
    protected override void OpenDoor()
    {
        StageManager.Instance.IsGamePause = false;
        RoomManager.Instance.PlaceNextRoom();
    }

    protected override void EnterRoom()
    {
        RoomManager.Instance.CurRoom = this;
    }

    protected override void ExitRoom()
    {
        RoomManager.Instance.PrevRoom = this;
    }

    public override void ResetRoom()
    {
        Destroy(gameObject);
    }
}
