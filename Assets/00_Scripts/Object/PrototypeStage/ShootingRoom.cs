using UnityEngine;

public class ShootingRoom : Room
{
    private RoomSO data;
    private Transform[] targetPoints;

    private void OnTriggerExit(Collider other)
    {
        StageManager.Instance.RemainTime += 20f;
    }
}
