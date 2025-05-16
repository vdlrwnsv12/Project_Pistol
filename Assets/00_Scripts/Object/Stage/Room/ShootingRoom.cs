using System.Collections.Generic;
using System.Linq;
using DataDeclaration;
using UnityEngine;

public class ShootingRoom : Room
{
    [SerializeField] private Transform[] targetPoints;
    [SerializeField] private Transform[] civilianTargetPoints;
    [SerializeField] private GameObject[] activeWalls;

    private List<BaseTarget> curActiveTargets = new();
    
    public StageSO Data { get; set; }

    private void Awake()
    {
        if (endPoint == null)
        {
            endPoint = transform.FindDeepChildByName("EndPoint");
        }
        
        exitGate.Door.OpenDoor += OpenDoor;
        exitGate.Door.DoorClosed += ResetRoom;
        exitGate.OnPassingGate += ExitRoom;
        enterGate.OnPassingGate += EnterRoom;
    }

    private void OnEnable()
    {
        if (Data != null)
        {
            InitWall();
            InitTarget();
            InitCivilianTarget();
        }
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
        RoomManager.Instance.CurRoom = this;
        enterGate.Door.gameObject.SetActive(true);
        RoomManager.Instance.RoomChangedAction();
        StageManager.Instance.RemainTime += Constants.ADDITIONAL_STAGE_TIME;
    }

    protected override void ExitRoom()
    {
        RoomManager.Instance.PrevRoom = this;
        exitGate.Door.Close();
    }

    public override void ResetRoom()
    {
        for (var i = 0; i < activeWalls.Length; i++)
        {
            activeWalls[i].SetActive(false);
        }
        enterGate.Door.gameObject.SetActive(false);
        ReturnTargetToPool();
        gameObject.SetActive(false);
    }

    private void InitWall()
    {
        for (var i = 0; i < activeWalls.Length; i++)
        {
            activeWalls[i].SetActive(Data.WallPoints[i]);
        }
    }

    private void InitTarget()
    {
        var targetIDList = Data.Targets;
        var targetList = new List<TargetSO>();
        for (var i = 0; i < targetIDList.Length; i++)
        {
            var id = targetIDList[i];
            var targetSO = ResourceManager.Instance.Load<TargetSO>($"Data/SO/TargetSO/{id}");
            targetList.Add(targetSO);
        }
        targetList.OrderBy(x => Random.value).ToList();

        for (var i = 0; i < Data.RespawnPoints.Length; i++)
        {
            var respawnIndex = Data.RespawnPoints[i];
            BaseTarget prefab = null;
            switch (targetList[i].Type)
            {
                case (int)TargetType.LandTarget:
                    prefab = Resources.Load<BaseTarget>("Prefabs/Stage/Target/LandTarget");
                    break;
                case (int)TargetType.AerialTarget:
                    prefab = Resources.Load<BaseTarget>("Prefabs/Stage/Target/AerialTarget");
                    break;
            }

            var target = ObjectPoolManager.Instance.GetObject<BaseTarget>(prefab, targetPoints[respawnIndex].position, targetPoints[respawnIndex].rotation);
            target.InitData(targetList[i]);
            curActiveTargets.Add(target);
        }
    }

    private void InitCivilianTarget()
    {
        var civilianRespawnPoints = civilianTargetPoints.OrderBy(x => Random.value).Take(Data.CivilianRespawn).ToArray();
        for (var i = 0; i < civilianRespawnPoints.Length; i++)
        {
            civilianRespawnPoints[i].gameObject.SetActive(true);
        }
    }

    private void ReturnTargetToPool()
    {
        for (var i = 0; i < curActiveTargets.Count; i++)
        {
            ObjectPoolManager.Instance.ReturnToPool(curActiveTargets[i].gameObject);
        }
        curActiveTargets.Clear();
    }
}
