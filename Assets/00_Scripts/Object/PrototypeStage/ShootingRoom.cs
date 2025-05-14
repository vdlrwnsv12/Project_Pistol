using System.Collections.Generic;
using System.Linq;
using DataDeclaration;
using UnityEngine;

public class ShootingRoom : Room
{
    [SerializeField] private Transform[] targetPoints;
    private Transform[] activeWallPoints;

    private List<BaseTarget> targetList;
    
    public RoomSO Data { get; set; }

    protected override void Awake()
    {
        base.Awake();
        //targetList = new List<BaseTarget>();
        //RespawnTarget();
    }

    private void OnEnable()
    {
        // if (Data != null)
        // {
        //     InitTarget();
        // }
    }

    private void OnDisable()
    {
        // foreach (var target in targetList)
        // {
        //     target.gameObject.SetActive(false);
        // }
    }

    protected override void OpenDoor()
    {
        if (RoomManager.Instance.CurStageIndex == Constants.LAST_STAGE_INDEX &&
            RoomManager.Instance.CurRoomIndex == Constants.MAX_ROOM_INDEX)
        {
            StageManager.Instance.GameOver();
            return;
        }
        
        RoomManager.Instance.PlaceNextRoom();
    }

    protected override void EnterRoom()
    {
    }

    protected override void ExitRoom()
    {
        RoomManager.Instance.RoomChangedAction();
    }

    private void InitTarget()
    {
        var targetIDList = Data.Targets;
        var activeTarget = GetRandomActiveTargetPoint();
        
        for (var i = 0; i < targetIDList.Length; i++)
        {
            var data = ResourceManager.Instance.Load<TargetSO>($"Data/SO/TargetSO/{targetIDList[i]}");
            targetList[i].InitData(data);
            targetList[i].gameObject.SetActive(true);
        }
    }

    private BaseTarget[] GetRandomActiveTargetPoint()
    {
        return targetList.OrderBy(o => Random.value).Take(Data.Targets.Length).ToArray();
    }

    private void RespawnTarget()
    {
        for (var i = 0; i < targetPoints.Length; i++)
        {
            var targetResource = ResourceManager.Instance.Load<BaseTarget>("Prefabs/Target/LandTarget");
            var target = Instantiate(targetResource, targetPoints[i]);
            target.gameObject.SetActive(false);
            targetList.Add(target);
        }
    }
}
