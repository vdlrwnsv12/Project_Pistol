using UnityEngine;

public class StandbyRoom : Room
{
    public Transform RespawnPoint { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        RespawnPoint = transform.FindDeepChildByName("RespawnPoint");
    }

    private void OpenRewardUI()
    {
        UIManager.Instance.OpenPopupUI<PopupReward>();
        UIManager.ToggleMouseCursor(true);
        StageManager.Instance.IsGamePause = true;
        StageManager.Instance.Player.Controller.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (StageManager.Instance.roomCreator.CurStageIndex == 8 &&
                StageManager.Instance.roomCreator.CurRoomIndex == 3)
            {
                StageManager.Instance.GameOver();
                return;
            }
            OpenRewardUI();
            
            StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
            StageManager.Instance.roomCreator.DisablePrevRoom();
            StageManager.Instance.roomCreator.CurRoom = this;
            
            StageManager.Instance.roomCreator.UpdateStageIndex();
            StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
        }
    }
}