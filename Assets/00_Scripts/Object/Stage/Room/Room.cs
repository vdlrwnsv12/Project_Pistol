using UnityEngine;

public abstract class Room : MonoBehaviour
{
    [SerializeField] protected EnterGate enterGate;
    [SerializeField] protected ExitGate exitGate;
    
    protected Transform endPoint;
    
    public Transform EndPoint => endPoint;

    protected abstract void OpenDoor();
    protected abstract void EnterRoom();
    protected abstract void ExitRoom();
    public abstract void ResetRoom();
}
