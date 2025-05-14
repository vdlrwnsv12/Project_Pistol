public class StageScene : BaseScene
{
    public override void EnterScene()
    {
        UIManager.ToggleMouseCursor(false);
        RoomManager.Instance.InitRoom();
        StageManager.Instance.SpawnPlayer();
        UIManager.Instance.InitMainUI<HUDUI>();
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}