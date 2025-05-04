public class StartScene : BaseScene
{
    public override void EnterScene()
    {
        UIManager.Instance.InitUI<StartUI>();
        UIManager.Instance.OpenPopupUI<PopupSignIn>();
    }

    public override void ExitScene()
    {
        
    }
}