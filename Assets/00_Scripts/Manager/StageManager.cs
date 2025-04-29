using System;
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
    private float remainTime;
    
    private int headHitCount = 0;
    
    private int destroyTargetCombo = 0;
    private int maxDestroyTargetCombo = 0;
    private float quickShotTimer;
    private const float QUICK_SHOT_TIME = 2f;

    public Player Player { get; private set; }

    public int CurStageIndex { get; set; }
    public int CurRoomIndex { get; set; }
    
    public int GameScore { get; set; }
    public float RemainTime
    {
        get => Math.Max(0f, remainTime);
        set => remainTime = value;
    }
    
    public int MaxDestroyTargetCombo => maxDestroyTargetCombo;
    public bool IsQuickShot { get; set; }
    public float QuickShotTimer => quickShotTimer;
    public int ShotCount { get; set; }
    public int HitCount { get; set; }
    private PrototypeStageManager roomLoader;

    public float ShotAccuracy
    {
        get
        {
            if (ShotCount == 0)
            {
                return 0;
            }

            return (float)HitCount / (float)ShotCount * 100f;
        }
    }

    public float HeadShotAccuracy
    {
        get
        {
            if (HitCount == 0)
            {
                return 0;
            }

            return (float)headHitCount / (float)HitCount * 100f;
        }
    }

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

        isGamePause = false;
        RemainTime = 20f;
        IsQuickShot = false;
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
            remainTime -= Time.deltaTime;
        }

        if (IsQuickShot)
        {
            quickShotTimer += Time.deltaTime;
            if (quickShotTimer >= QUICK_SHOT_TIME)
            {
                quickShotTimer = 0f;
                IsQuickShot = false;
            }
        }

        if (hudUI != null)
        {
            hudUI.UpdateRealTimeChanges(GameScore, RemainTime, Player.Weapon.CurAmmo,
                Player.Weapon.MaxAmmo);
        }

        if (remainTime <= 0)
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
            resultUI.SetResultValue(GameScore, RemainTime,
                ShotAccuracy, HeadShotAccuracy, MaxDestroyTargetCombo);
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