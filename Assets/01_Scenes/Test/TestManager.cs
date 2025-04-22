using DataDeclaration;
using UnityEngine;

public class TestManager : SingletonBehaviour<TestManager>
{
    private Player player;
    private HUDUI hudUI;
    private Vector3 respawnPos;
    
    protected override void Awake()
    {
        base.Awake();
        respawnPos = Vector3.up * 3;
        
        InitPlayer(GameManager.Instance.selectedCharacter, GameManager.Instance.selectedWeapon);

        InitHUD();
    }

    private void Start()
    {
        hudUI.UpdateStatValue(player.StatHandler.Stat.RCL, player.StatHandler.Stat.HDL, player.StatHandler.Stat.STP, player.StatHandler.Stat.SPD);
    }

    private void Update()
    {
        #region 로그인 테스트 디버깅 로그

        // Debug.Log($"접속 토큰: {UGSManager.Instance.AccessToken}");
        // Debug.Log($"플레이어 ID: {UGSManager.Instance.PlayerID}");
        // Debug.Log($"플레이어 이름: {UGSManager.Instance.PlayerName}");
        //
        // if (AuthenticationService.Instance.IsSignedIn)
        // {
        //     Debug.Log("로그인 상태입니다.");
        // }
        // else
        // {
        //     Debug.Log("아직 로그인하지 않았습니다.");
        // }

        #endregion
        
        hudUI.UpdateRealTimeChanges(0, 0, player.Weapon.CurAmmo, player.Weapon.MaxAmmo);
    }

    private void InitPlayer(CharacterSO character, WeaponSO weapon)
    {
        var resource = ResourceManager.Instance.Load<Player>($"Prefabs/Character/{character.ID}");
        player = Instantiate(resource, respawnPos, Quaternion.identity);
        
        player.InitWeapon(weapon.ID);
    }

    private void InitHUD()
    {
        UIManager.Instance.InitUI<HUDUI>();
        UIManager.Instance.ChangeMainUI(MainUIType.HUD);
        hudUI = UIManager.Instance.CurMainUI as HUDUI;
    }
}
