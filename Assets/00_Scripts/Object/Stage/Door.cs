using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteract
{
    public event Action OpenDoorAction;

    public void Interact()
    {
        gameObject.SetActive(false);
        OpenDoorAction?.Invoke();
    }
}