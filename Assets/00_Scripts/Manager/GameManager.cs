using Unity.Services.Authentication;
using UnityEngine;

public sealed class GameManager : SingletonBehaviour<GameManager>
{
    public CharacterSO selectedCharacter;
    public WeaponSO selectedWeapon;
    
    private string accessToken;
    private string playerID;
    private string playerName;

    private void Start()
    {
        AuthenticationService.Instance.SignOut();
        SetPlayerData();
    }

    private void Update()
    {
        Debug.Log($"접속 토큰: {accessToken}");
        Debug.Log($"플레이어 ID: {playerID}");
        Debug.Log($"플레이어 이름: {playerName}");
        
        if (AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("로그인 상태입니다.");
        }
        else
        {
            Debug.Log("아직 로그인하지 않았습니다.");
        }
    }

    public void SetPlayerData()
    {
        accessToken = AuthenticationService.Instance.AccessToken;
        playerID = AuthenticationService.Instance.PlayerId;
        playerName = AuthenticationService.Instance.PlayerName;
    }

    public static void LogOut()
    {
        AuthenticationService.Instance.SignOut();
        GameManager.Instance.SetPlayerData();
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
