using UnityEngine;

public sealed class GameManager : SingletonBehaviour<GameManager>
{
    public CharacterSO selectedCharacter;
    public WeaponSO selectedWeapon;

    public static bool isPuased = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameState();
        }
    }


    public static void ToggleGameState()
    {
        if (!isPuased)
        {
            Time.timeScale = 0;
            isPuased = true;
            UIManager.Instance.OpenPopUpUI("OptionPanel");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            isPuased = false;
            UIManager.Instance.ClosePopUpUI();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public static void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
