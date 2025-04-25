using DataDeclaration;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Singleton

    private static StageManager instance;

    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<StageManager>();
                if (instance == null)
                {
                    var go = new GameObject
                    {
                        name = nameof(StageManager)
                    };
                    instance = go.AddComponent<StageManager>();
                }
            }

            return instance;
        }
    }

    #endregion
    
    public Player Player { get; set; }
    
    public int CurStageIndex { get; set; }
    public int CurRoomIndex { get; set; }
    
    private StageLoader roomLoader;
    
    public RewardSystem RewardSystem { get; set; }
    //private HitTracker hitTracker;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SpawnPlayer(GameManager.Instance.selectedWeapon);
        
        RewardSystem = new RewardSystem();
    }

    private void Start()
    {
        InitHUDUI();
    }

    /// <summary>
    /// 캐릭터 생성
    /// </summary>
    private void SpawnPlayer(WeaponSO selectedWeapon)
    {
        var resource = ResourceManager.Instance.Load<Player>("Prefabs/Character/Player");
        Player = Instantiate(resource);
        Player.InitWeapon(selectedWeapon.ID);
    }

    private void InitHUDUI()
    {
        UIManager.Instance.InitUI<HUDUI>();
        UIManager.Instance.ChangeMainUI(MainUIType.HUD);
        var hudUI = UIManager.Instance.CurMainUI as HUDUI;
        if (hudUI != null)
        {
            hudUI.UpdateRealTimeChanges(0, 999, Player.Weapon.CurAmmo, Player.Weapon.MaxAmmo);
            hudUI.UpdateStageInfo(CurStageIndex, CurRoomIndex);
            hudUI.UpdateStatValue(Player.Stat.RCL, Player.Stat.HDL, Player.Stat.STP, Player.Stat.SPD);
        }
    }
}