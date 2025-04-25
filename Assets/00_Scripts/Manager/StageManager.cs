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
    
    public Player Player { get; private set; }
    
    public int CurStageIndex { get; set; }
    public int CurRoomIndex { get; set; }
    
    private StageLoader roomLoader;
    
    public RewardSystem RewardSystem { get; private set; }
    //private HitTracker hitTracker;

    private HUDUI hudUI;

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
        UIManager.ToggleMouseCursor(false);
        
        InitHUDUI();
    }

    private void Update()
    {
        if (hudUI != null)
        {
            hudUI.UpdateRealTimeChanges(0, 999, Player.Weapon.CurAmmo, Player.Weapon.MaxAmmo);
        }
    }

    public void GameOver()
    {
        UIManager.ToggleMouseCursor(true);
        
        UIManager.Instance.InitUI<ResultUI>();
        UIManager.Instance.ChangeMainUI(MainUIType.Result);
        var resultUI = UIManager.Instance.CurMainUI as ResultUI;
        if (resultUI != null)
        {
            //TODO: HitTracker 완성 시 작업
            //resultUI.SetResultValue(RankType.S, 9999, 99, 90, 9, 5);
        }
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
        hudUI = UIManager.Instance.CurMainUI as HUDUI;
        if (hudUI != null)
        {
            //TODO: 총기 사진 리소스 연결
            //hudUI.SetEquipImage();
            hudUI.UpdateStageInfo(CurStageIndex, CurRoomIndex);
            hudUI.UpdateStatValue(Player.Stat.RCL, Player.Stat.HDL, Player.Stat.STP, Player.Stat.SPD);
        }
    }
}