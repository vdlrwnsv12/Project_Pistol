using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDeclaration;

public class UIManager : SingletonBehaviour<UIManager>
{
    private GameObject mainCanvas; // 메인 캔버스 게임오브젝트
    private CanvasGroup fader; // 페이드 연출

    private List<MainUI> mainUIList = new(); // ScreenUI 관리용 리스트
    private Stack<GameObject> curPopUpUIStack = new(); // Pop-Up UI 관리용 Stack
    private Dictionary<string, GameObject> popUpUIPool = new(); // 비활성화 된 Pop-Up UI Pool

    protected override void Awake()
    {
        base.Awake();
        InitMainCanvas();
        InitFader();
    }

    /// <summary>
    /// ScreenUI를 상속받은 UI 클래스 생성 및 초기화
    /// </summary>
    /// <typeparam name="T">ScreenUI 클래스</typeparam>
    public void InitUI<T>() where T : MainUI
    {
        //TODO: Resources.Load 나중에 Addressable로 바꾸기
        var resource = Resources.Load<GameObject>($"Prefabs/UI/Main/{typeof(T).Name}");
        var ui = Instantiate(resource, mainCanvas.transform, false);
        mainUIList.Add(ui.GetComponent<T>());
    }

    /// <summary>
    /// ScreenUI 전환
    /// </summary>
    /// <param name="activeUIType">전환하려고 하는 ScreenUI 타입</param>
    public void ChangeMainUI(MainUIType activeUIType)
    {
        foreach (var screenUI in mainUIList)
        {
            screenUI.SetActiveUI(activeUIType);
        }
    }

    /// <summary>
    /// Pop-Up 창 열기
    /// </summary>
    /// <param name="openUI">활성화 할 Pop-Up 창</param>
    public void OpenPopUpUI<T>(T openUI) where T : PopupUI
    {
        if (curPopUpUIStack.TryPeek(out var latestUI))
        {
            latestUI.gameObject.SetActive(false);
        }
        
        var resource = Resources.Load<GameObject>($"Prefabs/UI/PopUp/{nameof(openUI)}");
        var ui = Instantiate(resource, mainCanvas.transform, false);
        curPopUpUIStack.Push(ui);
    }

    public void OpenPopUpUI<T>(string uiName) where T : PopupUI
    {
        if (curPopUpUIStack.TryPeek(out var latestUI))
        {
            latestUI.gameObject.SetActive(false);
        }
        
        var resource = Resources.Load<GameObject>($"Prefabs/UI/PopUp/{uiName}");
        var ui = Instantiate(resource, mainCanvas.transform, false);
        curPopUpUIStack.Push(ui);
    }

    /// <summary>
    /// Pop-Up 창 닫기
    /// </summary>
    public void ClosePopUpUI()
    {
        var popUpUI = curPopUpUIStack.Pop();
        popUpUI.gameObject.SetActive(false);

        if (curPopUpUIStack.TryPeek(out var prevUI))
        {
            prevUI.gameObject.SetActive(true);
        }
    }

    private GameObject FindPopUpUIInPool(PopupUI searchUI)
    {
        return null;
    }

    /// <summary>
    /// Main Canvas 초기화
    /// </summary>
    private void InitMainCanvas()
    {
        mainCanvas = GameObject.Find("MainCanvas"); // Main Canvas 찾기

        if (mainCanvas == null) // Main Canvas가 없을 경우 새로 생성
        {
            //TODO: Resources.Load 나중에 Addressable로 바꾸기
            var resource = Resources.Load<GameObject>("Prefabs/UI/MainCanvas");
            mainCanvas = Instantiate(resource);
        }

        DontDestroyOnLoad(mainCanvas);
    }

    /// <summary>
    /// Fader 초기화
    /// </summary>
    private void InitFader()
    {
        fader = (CanvasGroup)FindAnyObjectByType(typeof(CanvasGroup)); // Fader 찾기

        if (fader == null) // Fader가 없을 경우 새로 생성
        {
            //TODO: Resources.Load 나중에 Addressable롤 바꾸기
            var resource = Resources.Load<GameObject>("Prefabs/UI/Fader");
            fader = Instantiate(resource).GetComponent<CanvasGroup>();
        }

        DontDestroyOnLoad(fader.gameObject);
    }

    /// <summary>
    /// 마우스 커서 On/Off
    /// </summary>
    /// <param name="isActivation">True: 마우스 커서 활성화
    /// <para>False: 마우스 커서 비활성화</para></param>
    public static void ToggleMouseCursor(bool isActivation)
    {
        Cursor.lockState = isActivation ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /// <summary>
    /// Fade In/Out 효과
    /// </summary>
    /// <param name="startAlpha">시작 알파값</param>
    /// <param name="endAlpha">최종 알파값</param>
    /// <param name="duration">지속 시간</param>
    public IEnumerator FadeEffect(float startAlpha, float endAlpha, float duration)
    {
        var elapsedTime = 0f;
        fader.alpha = startAlpha;
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            fader.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        fader.alpha = endAlpha;
    }
}