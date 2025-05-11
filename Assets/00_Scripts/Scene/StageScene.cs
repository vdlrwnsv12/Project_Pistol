public class StageScene : BaseScene
{
    public override void EnterScene()
    {
        UIManager.ToggleMouseCursor(false);
        StageManager.Instance.InitStage();
        UIManager.Instance.InitMainUI<HUDUI>();
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}