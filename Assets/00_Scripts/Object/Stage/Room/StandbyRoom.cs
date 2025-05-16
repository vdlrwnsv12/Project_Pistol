using System.Collections;
using DataDeclaration;
using UnityEngine;

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
        
        StageManager.Instance.IsGamePause = true;
        StartCoroutine(OpenRewardUI(1f));
    }

    public override void ResetRoom()
    {
        StartCoroutine(DisableRoom(1f));
    }

    private IEnumerator DisableRoom(float time)
    {
        yield return new WaitForSeconds(time);
        enterGate.Door.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator OpenRewardUI(float time)
    {
        yield return new WaitForSeconds(time);
        UIManager.Instance.OpenPopupUI<PopupReward>();
        StageManager.Instance.Player.Controller.enabled = false;
        UIManager.ToggleMouseCursor(true);
    }
}