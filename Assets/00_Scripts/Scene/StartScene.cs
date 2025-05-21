public class StartScene : BaseScene
{
    public override void EnterScene()
    {
        UIManager.ToggleMouseCursor(true);
        UIManager.Instance.InitMainUI<StartUI>();
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}