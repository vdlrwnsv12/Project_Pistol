using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Scene = DataDeclaration.Scene;


public sealed class SceneLoadManager : SingletonBehaviour<SceneLoadManager>
{
    public static Scene CurScene { get; private set; } = Scene.Stage;
    public static Scene PrevScene { get; private set; }

    private Dictionary<Scene, BaseScene> scenes;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Start()
    {
        scenes[CurScene].EnterScene();
    }

    public IEnumerator LoadScene(Scene nextScene)
    {
        yield return null;

        var op = SceneManager.LoadSceneAsync((int)nextScene);
        op.allowSceneActivation = false;
        
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                
            }
            else
            {
                op.allowSceneActivation = true;
                
                PrevScene = CurScene;
                CurScene = nextScene;
                
                scenes[PrevScene].ExitScene();
                scenes[CurScene].EnterScene();
            }
        }
    }

    private void Init()
    {
        scenes = new Dictionary<Scene, BaseScene>();
        scenes.Add(Scene.Start, new StartScene());
        scenes.Add(Scene.Lobby, new LobbyScene());
        scenes.Add(Scene.Stage, new StageScene());
    }
}