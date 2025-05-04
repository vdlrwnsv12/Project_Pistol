using System;
using DataDeclaration;
using UnityEngine;

public class StageManager : SingletonBehaviour<StageManager>
{
    public bool IsGamePause;
    private float remainTime;
    
    private int maxDestroyTargetCombo = 0;
    private float quickShotTimer;

    public Player Player { get; private set; }
    
    public int GameScore { get; set; }
    public float RemainTime
    {
        get => Math.Max(0f, remainTime);
        set => remainTime = value;
    }
    
    public int DestroyTargetCombo { get; set; }
    public int MaxDestroyTargetCombo { get; set; }
    public bool IsQuickShot { get; set; }
    public float QuickShotTimer
    {
        set => quickShotTimer = value;
    }
    public int ShotCount { get; set; }
    public int HitCount { get; set; }
    public int HeadHitCount { get; set; }
    public RoomCreator roomCreator;

    private float shotAccuracy;
    private float headShotAccuracy;

    public HUDUI HUDUI {get; private set;}

    protected override void Awake()
    {
        isDestroyOnLoad = true;
        base.Awake();
        IsGamePause = true;
        RemainTime = 20f;
        IsQuickShot = false;
    }

    private void Start()
    {
        UIManager.ToggleMouseCursor(false);

        var stagePoint = transform;
        roomCreator.CurRoom = roomCreator.PlaceStandbyRoom(stagePoint);
        roomCreator.NextRoom = roomCreator.PlaceShootingRoom(roomCreator.CurRoom.EndPoint, 0);
        
        SpawnPlayer(roomCreator.StandbyRoom.RespawnPoint.position);
        
        InitHUDUI();
    }

    private void Update()
    {
        if (!IsGamePause)
        {
            remainTime -= Time.deltaTime;
        }

        if (IsQuickShot)
        {
            quickShotTimer += Time.deltaTime;
            if (quickShotTimer >= Constants.QUICK_SHOT_TIME)
            {
                quickShotTimer = 0f;
                IsQuickShot = false;
                Debug.Log("큇샷 시간 초과");
            }
        }

        if (HUDUI != null)
        {
            HUDUI.UpdateRealTimeChanges(GameScore, RemainTime, Player.Weapon.CurAmmo,
                Player.Weapon.MaxAmmo);
        }

        if (remainTime <= 0)
        {
            GameOver();
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
            CalculateAccuracy();
            resultUI.SetResultValue(GameScore, RemainTime,
                shotAccuracy, headShotAccuracy, MaxDestroyTargetCombo);
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
        HUDUI = UIManager.Instance.CurMainUI as HUDUI;
        if (HUDUI != null)
        {
            //TODO: 총기 사진 리소스 연결
            //hudUI.SetEquipImage();
            HUDUI.UpdateStageInfo(roomCreator.CurStageIndex, roomCreator.CurRoomIndex);
            HUDUI.UpdateStatValue(Player.Stat.RCL, Player.Stat.HDL, Player.Stat.STP, Player.Stat.SPD);
        }
    }

    private void CalculateAccuracy()
    {
        if (ShotCount == 0)
        {
            shotAccuracy = 0;
        }
        else
        {
            shotAccuracy = (float)HitCount / (float)ShotCount * 100f;
        }

        if (HitCount == 0)
        {
            headShotAccuracy = 0;
        }
        else
        {
            headShotAccuracy = (float)HeadHitCount / (float)HitCount * 100f;
        }
    }
}