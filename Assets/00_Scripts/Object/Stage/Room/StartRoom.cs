using System.Collections;
using UnityEngine;

public class StartRoom : Room
{
    protected override void Awake()
    {
        base.Awake();
        if (endPoint == null)
        {
            endPoint = transform.FindDeepChildByName("EndPoint");
        }
        exitGate.Door.OpenDoor += OpenDoor;
        exitGate.OnPassingGate += ExitRoom;
    }

    private void Start()
    {
        EnterRoom();
    }

    protected override void OpenDoor()
    {
        base.OpenDoor();
        
        StageManager.Instance.IsGamePause = false;
    }

    protected override void EnterRoom()
    {
        RoomManager.Instance.CurRoom = this;
    }

    protected override void ResetRoom()
    {
        StartCoroutine(DestroyRoom(1f));
    }

    public override bool CanOpenDoor()
    {
        return true;
    }

    private IEnumerator DestroyRoom(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
