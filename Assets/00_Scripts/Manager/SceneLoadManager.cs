using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = DataDeclaration.Scene;

public sealed class SceneLoadManager : SingletonBehaviour<SceneLoadManager>
{
    public static Scene CurScene { get; private set; } = Scene.Start;
    public static Scene PrevScene { get; private set; }
    public static Scene NextScene { get; private set; }

    private Dictionary<Scene, BaseScene> scenes;
    
    public event Action<float> OnLoadProgress;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Start()
    {
        scenes[CurScene].EnterScene();
    }

    public void LoadScene(Scene scene)
    {
        NextScene = scene;
        //SceneManager.LoadScene((int)Scene.Loading);
        StartCoroutine(CoroutineLoadScene(scene));
    }

    private IEnumerator CoroutineLoadScene(Scene nextScene)
    {
        yield return UIManager.Instance.FadeEffect(0, 1, 0.5f);
        UIManager.Instance.InitMainUI<LoadingUI>();
        yield return UIManager.Instance.FadeEffect(1, 0, 0.5f);

        var op = SceneManager.LoadSceneAsync((int)nextScene);
        op.allowSceneActivation = false;
        
        while (!op.isDone)
        {
            yield return null;
            OnLoadProgress?.Invoke(op.progress);

            if (op.progress < 0.9f)
            {
                
            }
            else
            {
                op.allowSceneActivation = true;

                PrevScene = CurScene;
                CurScene = nextScene;

                yield return new WaitForSeconds(1.5f);
                yield return UIManager.Instance.FadeEffect(0, 1, 0.5f);
                scenes[PrevScene].ExitScene();
                scenes[CurScene].EnterScene();
                yield return UIManager.Instance.FadeEffect(1, 0, 0.5f);
                break;
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