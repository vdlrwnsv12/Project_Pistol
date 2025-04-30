using UnityEngine;

public static class GameStateManager
{
    public static bool isPaused = false;

    public static void ToggleGameState()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            UIManager.Instance.OpenPopupUI<PopupOption>();
            UIManager.ToggleMouseCursor(true);
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
            UIManager.Instance.ClosePopUpUI();
            UIManager.ToggleMouseCursor(false);
        }
    }
}
