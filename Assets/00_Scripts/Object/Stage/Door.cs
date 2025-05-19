using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteract
{
    private static readonly int OpenDoorTrigger = Animator.StringToHash("OpenDoorTrigger");
    private static readonly int CloseDoorTrigger = Animator.StringToHash("CloseDoorTrigger");
    public event Action OpenDoor;
    public event Action DoorOpened;
    public event Action CloseDoor;
    public event Action DoorClosed;

    public bool IsOpened { get; private set; }
    
    private Animator animator;

    private void Awake()
    {
        IsOpened = false;
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (RoomManager.Instance.CurRoom.CanOpenDoor())
        {
            Open();
        }
    }

    private void Open()
    {
        if (!IsOpened)
        {
            IsOpened = true;
            animator.SetTrigger(OpenDoorTrigger);
            OpenDoor?.Invoke();
        }
    }

    public void Close()
    {
        animator.SetTrigger(CloseDoorTrigger);
        CloseDoor?.Invoke();
    }

    private void Opened()
    {
        DoorOpened?.Invoke();
    }

    private void Closed()
    {
        IsOpened = false;
        DoorClosed?.Invoke();
    }
}