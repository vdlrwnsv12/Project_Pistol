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
            StageManager.Instance.CurStageIndex++;
            StageManager.Instance.HUDUI.UpdateStageInfo(StageManager.Instance.CurStageIndex, RoomIndex);
        }
    }
}