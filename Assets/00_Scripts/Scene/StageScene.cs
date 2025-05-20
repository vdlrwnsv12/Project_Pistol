using UnityEngine;

public class StageScene : BaseScene
{
    public override void EnterScene()
    {
        UIManager.ToggleMouseCursor(false);
        RoomManager.Instance.InitRoom();
        StageManager.Instance.SpawnPlayer();
        UIManager.Instance.InitMainUI<HUDUI>();
        var bgm = ResourceManager.Instance.Load<AudioClip>("Audio/BGM/Can't Stop Me_demo");
        SoundManager.Instance.PlayBackgroundMusic(bgm);
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}