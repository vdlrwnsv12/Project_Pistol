using UnityEngine;

public class ShootingRoom : Room
{
    private RoomSO data;
    private Transform[] targetPoints;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.RemainTime += 20f;
        }
    }
}
