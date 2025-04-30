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
            
            StageManager.Instance.roomCreator.PrevRoom = StageManager.Instance.roomCreator.CurRoom;
            StageManager.Instance.roomCreator.CurRoom = this;
            
            StageManager.Instance.roomCreator.UpdateStageIndex();
            
            if (StageManager.Instance.roomCreator.CurRoomIndex == 3)
            {
                StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceStandbyRoom(endPoint);
            }
            else
            {
                StageManager.Instance.roomCreator.NextRoom = StageManager.Instance.roomCreator.PlaceShootingRoom(endPoint, StageManager.Instance.roomCreator.CurRoomIndex);
            }
            StageManager.Instance.roomCreator.DisablePrevRoom();
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
