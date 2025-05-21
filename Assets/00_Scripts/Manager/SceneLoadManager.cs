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

    private bool isInitialized = false;

    protected override void Awake()
    {
        base.Awake();
        isDontDestroyOnLoad = true;
        Init();
    }

    private void Start()
    {
        scenes[CurScene].EnterScene();
    }

    public void LoadScene(Scene scene)
    {
        UIManager.Instance.ClosePopUpUI();//팝업 창 있으면 닫기(여기에 쓰는게 맞는진 모르겠음)
        PrevScene = CurScene;
        NextScene = scene;
        CurScene = Scene.Loading;
        Debug.Log($"[SceneLoadManager] LoadScene 호출됨 → Prev: {PrevScene}, Next: {NextScene}");

        SceneManager.LoadScene((int)Scene.Loading, LoadSceneMode.Single);

        //StartCoroutine(CoroutineLoadScene(scene));
    }

    // private IEnumerator CoroutineLoadScene(Scene nextScene)
    // {
    //     yield return null;

    //     var op = SceneManager.LoadSceneAsync((int)nextScene);
    //     op.allowSceneActivation = false;

    //     while (!op.isDone)
    //     {
    //         yield return null;

    //         if (op.progress < 0.9f)
    //         {

    //         }
    //         else
    //         {
    //             op.allowSceneActivation = true;

    //             PrevScene = CurScene;
    //             CurScene = nextScene;

    //             scenes[PrevScene].ExitScene();
    //             scenes[CurScene].EnterScene();
    //             break;
    //         }
    //     }
    // }
    public void OnSceneEnterComplete()
    {
        CurScene = NextScene;

        if (scenes.TryGetValue(PrevScene, out var prevSceneInstance))
        {
            Debug.Log($"[SceneLoadManager] ExitScene 호출: {PrevScene}");
            prevSceneInstance.ExitScene();
        }

        if (scenes.TryGetValue(CurScene, out var curSceneInstance))
        {
            Debug.Log($"[SceneLoadManager] EnterScene 호출: {CurScene}");
            curSceneInstance.EnterScene();
        }
        else
        {
            Debug.LogWarning($"[SceneLoadManager] CurScene {CurScene} not found in scenes dictionary");
        }

         Debug.Log($"[SceneLoadManager] OnSceneEnterComplete 호출됨, 인스턴스 유효: {Instance != null}");

    }


    private void Init()
    {
        if (isInitialized) return;
        isInitialized = true;


        scenes = new Dictionary<Scene, BaseScene>();
        scenes.Add(Scene.Start, new StartScene());
        scenes.Add(Scene.Lobby, new LobbyScene());
        scenes.Add(Scene.Stage, new StageScene());
        scenes.Add(Scene.Loading, new LoadingScene());
        Debug.Log("[SceneLoadManager] Scene dictionary 초기화 완료");
    }
}