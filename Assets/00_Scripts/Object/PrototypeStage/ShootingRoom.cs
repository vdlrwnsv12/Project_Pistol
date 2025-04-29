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

    private void RespawnTarget()
    {
        var targetIDList = data.Targets;
        
        for (var i = 0; i < targetIDList.Length; i++)
        {
            var targetResource = ResourceManager.Instance.Load<GameObject>($"Prefabs/Target/LandTarget");
            Instantiate(targetResource, targetPoints[i]);
        }
    }
}
