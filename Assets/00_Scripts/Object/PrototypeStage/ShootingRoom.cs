using System.Collections.Generic;
using System.Linq;
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
        targetList = new List<BaseTarget>();
        RespawnTarget();
    }

    private void OnEnable()
    {
        if (Data != null)
        {
            InitTarget();
        }
    }

    private void OnDisable()
    {
        foreach (var target in targetList)
        {
            target.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.IsGamePause = false;
            StageManager.Instance.RemainTime += 20f;
            
            StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
            StageManager.Instance.roomCreator.DisablePrevRoom();
            StageManager.Instance.roomCreator.CurRoom = this;
            
            StageManager.Instance.roomCreator.UpdateStageIndex();
            
            if (StageManager.Instance.roomCreator.CurRoomIndex == 3)
            {
                StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceStandbyRoom(endPoint);
            }
            else
            {
                StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
            }
        }
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
