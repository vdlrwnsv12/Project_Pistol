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

    private bool isOpened;
    
    private Animator animator;

    private void Awake()
    {
        isOpened = false;
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        Open();
    }

    private void Open()
    {
        animator.SetTrigger(OpenDoorTrigger);
        OpenDoor?.Invoke();
    }

    private void Close()
    {
        animator.SetTrigger(CloseDoorTrigger);
        CloseDoor?.Invoke();
    }

    private void Opened()
    {
        isOpened = true;
        DoorOpened?.Invoke();
    }

    private void Closed()
    {
        isOpened = false;
        DoorClosed?.Invoke();
    }
}