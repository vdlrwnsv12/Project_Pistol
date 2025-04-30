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
        StageManager.Instance.PauseGame(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenRewardUI();
            
            StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
            StageManager.Instance.roomCreator.CurRoom = this;
            
            StageManager.Instance.roomCreator.UpdateStageIndex();
            StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
            
            StageManager.Instance.roomCreator.DisablePrevRoom();
        }
    }
}