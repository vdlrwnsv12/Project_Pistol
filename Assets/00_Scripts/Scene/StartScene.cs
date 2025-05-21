using UnityEngine;

public class StartScene : BaseScene
{
    public override void EnterScene()
    {
        UIManager.ToggleMouseCursor(true);
        UIManager.Instance.InitMainUI<StartUI>();
        var bgm = ResourceManager.Instance.Load<AudioClip>("Audio/BGM/Let's Rock_demo");
        SoundManager.Instance.PlayBackgroundMusic(bgm);
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}