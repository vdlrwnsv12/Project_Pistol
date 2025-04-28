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

    private bool isGamePause;

    public Player Player { get; private set; }

    public int CurStageIndex { get; set; }
    public int CurRoomIndex { get; set; }

    private StageLoader roomLoader;

    public RewardSystem RewardSystem { get; private set; }
    public HitTracker HitTracker { get; private set; }

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

        SpawnPlayer(GameManager.Instance.respawnPoint.position);

        RewardSystem = new RewardSystem();
        HitTracker = new HitTracker();

        isGamePause = false;
    }

    private void Start()
    {
        UIManager.ToggleMouseCursor(false);

        InitHUDUI();
    }

    private void Update()
    {
        if (!isGamePause)
        {
            HitTracker.RemainTime -= Time.deltaTime;
        }

        if (hudUI != null)
        {
            hudUI.UpdateRealTimeChanges(HitTracker.GameScore, HitTracker.RemainTime, Player.Weapon.CurAmmo,
                Player.Weapon.MaxAmmo);
        }

        if (HitTracker.RemainTime <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        UIManager.ToggleMouseCursor(true);

        UIManager.Instance.InitUI<ResultUI>();
        UIManager.Instance.ChangeMainUI(MainUIType.Result);
        var resultUI = UIManager.Instance.CurMainUI as ResultUI;
        if (resultUI != null)
        {
            resultUI.SetResultValue(HitTracker.Rank, HitTracker.GameScore, HitTracker.RemainTime,
                HitTracker.ShotAccuracy, HitTracker.HeadShotAccuracy, HitTracker.MaxDestroyTargetCombo);
        }

        Player.Controller.enabled = false;
    }

    /// <summary>
    /// 캐릭터 생성
    /// </summary>
    private void SpawnPlayer(Vector3 spawnPoint)
    {
        var resource = ResourceManager.Instance.Load<Player>("Prefabs/Character/Player");
        Player = Instantiate(resource, spawnPoint, Quaternion.identity);
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

    public void PauseGame(bool isPause)
    {
        isGamePause = isPause;
        if (Player.Controller != null)
        {
            Player.Controller.enabled = !isGamePause;
        }
    }
}