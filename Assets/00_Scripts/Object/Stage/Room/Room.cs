using System;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    [SerializeField] protected EnterGate enterGate;
    [SerializeField] protected ExitGate exitGate;
    
    protected Transform endPoint;
    
    public Transform EndPoint => endPoint;
    
    public ExitGate ExitGate => exitGate;

    protected virtual void Awake()
    {
        exitGate.Door.DoorClosed += ResetRoom;
    }

    protected virtual void OpenDoor()
    {
        RoomManager.Instance.PlaceNextRoom();
    }

    protected virtual void EnterRoom()
    {
        RoomManager.Instance.CurRoom = this;
    }

    protected virtual void ExitRoom()
    {
        RoomManager.Instance.PrevRoom = this;
        exitGate.Door.Close();
    }
    
    public abstract void ResetRoom();
}
