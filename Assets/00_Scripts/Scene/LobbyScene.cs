using UnityEngine;

public class LobbyScene : BaseScene
{
    public override void EnterScene()
    {
        GameManager.Instance.selectedCharacter = null;
        GameManager.Instance.selectedWeapon = null;

        var bgm = ResourceManager.Instance.Load<AudioClip>("Audio/BGM/No Way Back_demo");
        SoundManager.Instance.PlayBackgroundMusic(bgm);
        UIManager.Instance.InitMainUI<LobbyUI>();
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}