public class StartScene : BaseScene
{
    public override void EnterScene()
    {
        UIManager.ToggleMouseCursor(true);
        UIManager.Instance.InitMainUI<StartUI>();
        UIManager.Instance.OpenPopupUI<PopupSignIn>();
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}