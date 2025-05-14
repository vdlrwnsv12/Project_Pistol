using UnityEngine;

public abstract class Room : MonoBehaviour
{
    public Door exitDoor;
    
    protected Transform endPoint;
    
    public Transform EndPoint => endPoint;

    protected virtual void Awake()
    {
        if (endPoint == null)
        {
            endPoint = transform.FindDeepChildByName("EndPoint");
        }

        exitDoor = GetComponentInChildren<Door>();
    }
}
