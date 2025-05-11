using System;
using DataDeclaration;
using UnityEngine;

public class StageManager : SingletonBehaviour<StageManager>
{
    public bool IsGamePause { get; set; }
    private float remainTime;
    
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

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
        IsGamePause = true;
        RemainTime = Constants.INIT_STAGE_TIME;
        IsQuickShot = false;
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

        if (remainTime <= 0)
        {
            GameOver();
        }
    }
    
    public void InitStage()
    {
        var stagePoint = transform;
        roomCreator.CurRoom = roomCreator.PlaceStandbyRoom(stagePoint);
        roomCreator.NextRoom = roomCreator.PlaceShootingRoom(roomCreator.CurRoom.EndPoint, 0);
        
        SpawnPlayer(roomCreator.StandbyRoom.RespawnPoint.position);
    }

    public void GameOver()
    {
        UIManager.ToggleMouseCursor(true);

        UIManager.Instance.InitMainUI<ResultUI>();
        var resultUI = UIManager.Instance.CurMainUI as ResultUI;
        if (resultUI != null)
        {
            shotAccuracy = ShotCount == 0 ? 0 : (float)HitCount / ShotCount * 100f;
            headShotAccuracy = HitCount == 0 ? 0 : (float)HeadHitCount / HitCount * 100f;
            
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
}