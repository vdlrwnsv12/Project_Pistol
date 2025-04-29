using UnityEngine;

public abstract class Room : MonoBehaviour
{
    protected Transform startPoint;
    protected Transform endPoint;
    
    public Transform StartPoint => startPoint;
    public Transform EndPoint => endPoint;
    public int RoomIndex { get; set; }

    protected virtual void Awake()
    {
        if (startPoint == null)
        {
            startPoint = transform.FindDeepChildByName("StartPoint");
        }

        if (endPoint == null)
        {
            endPoint = transform.FindDeepChildByName("EndPoint");
        }
    }
}
