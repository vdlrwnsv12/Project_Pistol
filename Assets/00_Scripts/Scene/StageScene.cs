public class StageScene : BaseScene
{
    public override void EnterScene()
    {
        UIManager.ToggleMouseCursor(false);
        StageManager.Instance.InitStage();
        UIManager.Instance.InitUI<HUDUI>();
    }

    public override void ExitScene()
    {
    }
}