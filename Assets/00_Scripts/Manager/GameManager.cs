using Unity.Services.Authentication;
using UnityEngine;

public sealed class GameManager : SingletonBehaviour<GameManager>
{
    public CharacterSO selectedCharacter;
    public WeaponSO selectedWeapon;
    
    private string accessToken;
    private string playerID;
    private string playerName;
    
    public string AccessToken => accessToken;
    public string PlayerID => playerID;
    public string PlayerName => playerName;

    private void Start()
    {
        AuthenticationService.Instance.SignOut();
        SetPlayerData();
    }

    private void SetPlayerData()
    {
        accessToken = AuthenticationService.Instance.AccessToken;
        playerID = AuthenticationService.Instance.PlayerId;
        playerName = AuthenticationService.Instance.PlayerName;
    }

    public void LogOut()
    {
        AuthenticationService.Instance.SignOut();
        SetPlayerData();
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
