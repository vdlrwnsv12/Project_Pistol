using UnityEngine;

public class LobbyScene : BaseScene
{
    public override void EnterScene()
    {
        var bgm = ResourceManager.Instance.Load<AudioClip>("Audio/BGM/No Way Back_demo");
        SoundManager.Instance.PlayBackgroundMusic(bgm);
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}