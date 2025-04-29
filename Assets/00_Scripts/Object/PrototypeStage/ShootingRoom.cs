using UnityEngine;

public class ShootingRoom : Room
{
    [SerializeField] private Transform[] targetPoints;
    private Transform[] activeWallPoints;
    
    public RoomSO Data { get; set; }

    private void OnEnable()
    {
        if (Data != null)
        {
            RespawnTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.PauseGame(false);
            StageManager.Instance.RemainTime += 20f;
            StageManager.Instance.HUDUI.UpdateStageInfo(StageManager.Instance.CurStageIndex, RoomIndex);
            if (RoomIndex == 2)
            {
                var nextStage = StageManager.Instance.CurStageIndex + 1;
                StageManager.Instance.roomCreator.CreateStage(endPoint, nextStage);
            }
        }
    }

    private void RespawnTarget()
    {
        var targetIDList = Data.Targets;
        
        for (var i = 0; i < targetIDList.Length; i++)
        {
            var targetResource = ResourceManager.Instance.Load<GameObject>($"Prefabs/Target/LandTarget");
            Instantiate(targetResource, targetPoints[i]);
        }
    }
}
