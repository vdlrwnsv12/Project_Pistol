using System.Collections.Generic;
using System.Linq;
using DataDeclaration;
using UnityEngine;

public class ShootingRoom : Room
{
    [SerializeField] private Transform[] targetPoints;
    [SerializeField] private Transform[] civilianTargetPoints;
    [SerializeField] private GameObject[] activeWalls;
    
    private List<BaseTarget> targetList;
    
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
        gameObject.SetActive(false);
    }

    private void InitWall()
    {
        for (var i = 0; i < activeWalls.Length; i++)
        {
            activeWalls[i].SetActive(Data.WallPoints[i]);
        }
    }
}
