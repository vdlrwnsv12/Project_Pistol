using UnityEngine;

public sealed class GameManager : SingletonBehaviour<GameManager>
{
    public CharacterSO selectedCharacter;
    public WeaponSO selectedWeapon;

    public static bool isPaused = false;
    
    // private void Update()
    // {
    //     //TODO: InputAction으로 변경
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         ToggleGameState();
    //     }
    // }


    // public static void ToggleGameState()
    // {
    //     if (!isPaused)
    //     {
    //         Time.timeScale = 0;
    //         isPaused = true;
    //         UIManager.Instance.OpenPopupUI<PopupOption>();
    //         UIManager.ToggleMouseCursor(true);
    //     }
    //     else
    //     {
    //         Time.timeScale = 1;
    //         isPaused = false;
    //         UIManager.Instance.ClosePopUpUI();
    //         UIManager.ToggleMouseCursor(false);
    //     }
    // }

    public static void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}