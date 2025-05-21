#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public sealed class GameManager : SingletonBehaviour<GameManager>
{
    public CharacterSO selectedCharacter;
    public WeaponSO selectedWeapon;
    public bool isOption = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePopup();
        }

    }

    public void TogglePopup()
    {
        if (isOption)
        {
            isOption = false;
            UIManager.Instance.ClosePopUpUI();
            Time.timeScale = 1f;
        }
        else
        {
            isOption = true;
            UIManager.Instance.OpenPopupUI<PopupOption>();
            Time.timeScale = 0f;
        }
    }

    public static void GameQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
