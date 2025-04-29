using UnityEngine;

public class StandbyRoom : Room
{
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
        }
    }
}