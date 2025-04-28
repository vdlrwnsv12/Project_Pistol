using UnityEngine;

public static class RoomConnector
{
    public static void AlignRooms(Transform previousEnd, GameObject currentRoom)
    {
        Transform currStart = currentRoom.transform.Find("StartPoint");

        if (previousEnd != null && currStart != null)
        {
            Vector3 offset = previousEnd.position - currStart.position;
            currentRoom.transform.position += offset;
        }
    }
}
