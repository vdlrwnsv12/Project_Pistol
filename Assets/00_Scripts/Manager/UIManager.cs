using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDeclaration;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public sealed class UIManager : SingletonBehaviour<UIManager>
{
    private GameObject mainCanvas; // 메인 캔버스 게임오브젝트
    private GameObject popupCanvas; // 팝업 캔버스 게임오브젝트
    private CanvasGroup fader; // 페이드 연출
    
    private readonly List<MainUI> mainUIList = new(); // MainUI 관리용 리스트
    private readonly Stack<PopupUI> curPopupUIStack = new(); // Popup UI 관리용 Stack
    private readonly Dictionary<string, PopupUI> popupUIPool = new(); // 비활성화 된 Popup UI Pool
    
    public MainUI CurMainUI { get; private set; }   // 현재 활성화 된 Main UI
    public PopupUI CurPopupUI { get; private set; } // 마지막으로 활성화 된 Popup UI

    protected override void Awake()
    {
        base.Awake();
        InitMainCanvas();
        InitPopupCanvas();
        InitFader();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Public Method

    #region Main UI

    /// <summary>
    /// ScreenUI를 상속받은 UI 클래스 생성 및 초기화
    /// </summary>
    /// <typeparam name="T">ScreenUI 클래스</typeparam>
    public void InitUI<T>() where T : MainUI
    {
        var mainUI = mainUIList.Find(o => o.GetType() == typeof(T));
        if (mainUI == null)
        {
            //TODO: Resources.Load 나중에 Addressable로 바꾸기
            var resource = ResourceManager.Instance.Load<T>($"Prefabs/UI/Main/{typeof(T).Name}");
            mainUI = Instantiate(resource, mainCanvas.transform, false);
            mainUIList.Add(mainUI);
        }
        ChangeMainUI(mainUI.UIType);
    }

    /// <summary>
    /// Main UI 전환
    /// </summary>
    /// <param name="activeUIType">전환하려고 하는 Main UI 타입</param>
    public void ChangeMainUI(MainUIType activeUIType)
    {
        foreach (var mainUI in mainUIList)
        {
            mainUI.SetActiveUI(activeUIType);
            if (mainUI.gameObject.activeSelf)
            {
                CurMainUI = mainUI;
            }
        }
    }

    #endregion
    
    #region Popup UI
    /// <summary>
    /// Resources/Prefabs/UI/PopUp/ 경로에 있는 Popup UI 리소스 생성
    /// </summary>
    /// <typeparam name="T">PopupUI 클래스</typeparam>
    public void OpenPopupUI<T>() where T : PopupUI
    {
        // if (curPopupUIStack.TryPeek(out var latestUI))
        // {
        //     latestUI.gameObject.SetActive(false);
        // }

        var openUI = FindPopUpUIInPool<T>();
        if (openUI == null)
        {
            var resource = ResourceManager.Instance.Load<T>($"Prefabs/UI/PopUp/{typeof(T).Name}");
            openUI = Instantiate(resource, popupCanvas.transform, false);
        }

        openUI.gameObject.SetActive(true);
        openUI.transform.SetAsLastSibling();
        curPopupUIStack.Push(openUI);
        CurPopupUI = openUI;
    }

    /// <summary>
    /// Resources/Prefabs/UI/PopUp/ 경로에 있는 Popup UI 리소스 생성
    /// </summary>
    /// <param name="popUpUI">PopupUI를 상속받은 UI클래스</param>
    public void OpenPopUpUI(PopupUI popUpUI)
    {
        var uiName = popUpUI.GetType().Name;

        // if (curPopupUIStack.TryPeek(out var latestUI))
        // {
        //     latestUI.gameObject.SetActive(false);
        // }

        var openUI = FindPopUpUIInPool(uiName);
        if (openUI == null)
        {
            var resource = ResourceManager.Instance.Load<PopupUI>($"Prefabs/UI/PopUp/{uiName}");
            openUI = Instantiate(resource, popupCanvas.transform, false);
        }

        openUI.gameObject.SetActive(true);
        openUI.transform.SetAsLastSibling();
        curPopupUIStack.Push(openUI);
        CurPopupUI = openUI;
    }

    /// <summary>
    /// Resources/Prefabs/UI/PopUp/ 경로에 있는 Popup UI 리소스 생성
    /// </summary>
    /// <param name="uiName">리소스 이름</param>
    public void OpenPopUpUI(string uiName)
    {
        // if (curPopupUIStack.TryPeek(out var latestUI))
        // {
        //     latestUI.gameObject.SetActive(false);
        // }

        var openUI = FindPopUpUIInPool(uiName);
        if (openUI == null)
        {
            var resource = ResourceManager.Instance.Load<PopupUI>($"Prefabs/UI/PopUp/{uiName}");
            openUI = Instantiate(resource, popupCanvas.transform, false);
        }

        openUI.gameObject.SetActive(true);
        openUI.transform.SetAsLastSibling();
        curPopupUIStack.Push(openUI);
        CurPopupUI = openUI;
    }

    /// <summary>
    /// Pop-Up 창 닫기
    /// </summary>
    public void ClosePopUpUI()
    {
        if(curPopupUIStack.Count == 0)
        {
            Debug.LogWarning("팝 할거 없음");
            return;
        }
        var popUpUI = curPopupUIStack.Pop();
        popUpUI.gameObject.SetActive(false);
        CurPopupUI = null;

        var type = popUpUI.GetType();
        if (popupUIPool.ContainsKey(type.Name))
        {
            popupUIPool[type.Name] = popUpUI;
        }
        else
        {
            popupUIPool.Add(type.Name, popUpUI);
        }

        if (curPopupUIStack.TryPeek(out var prevUI))
        {
            prevUI.gameObject.SetActive(true);
            prevUI.transform.SetAsLastSibling();
            CurPopupUI = prevUI;
        }
    }
    
    #endregion

    /// <summary>
    /// 마우스 커서 On/Off
    /// </summary>
    /// <param name="isActivation">True: 마우스 커서 활성화
    /// <para>False: 마우스 커서 비활성화</para></param>
    public static void ToggleMouseCursor(bool isActivation)
    {
        Cursor.lockState = isActivation ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = isActivation;
    }

    /// <summary>
    /// Fade In/Out 효과
    /// </summary>
    /// <param name="startAlpha">시작 알파값</param>
    /// <param name="endAlpha">최종 알파값</param>
    /// <param name="duration">지속 시간</param>
    /// <param name="isReset">알파값 0 초기화 여부</param>
    public IEnumerator FadeEffect(float startAlpha, float endAlpha, float duration, bool isReset = false)
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
        
        if (isReset)
        {
            yield return new WaitForEndOfFrame();
            fader.alpha = 0;
        }
    }

    #endregion

    #region Private Method

    /// <summary>
    /// 비활성화 UI Pool에서 UI 검색
    /// </summary>
    /// <typeparam name="T">PopupUI 클래스</typeparam>
    /// <returns>비활성화된 Popup UI</returns>
    private T FindPopUpUIInPool<T>() where T : PopupUI
    {
        return popupUIPool.GetValueOrDefault(typeof(T).Name) as T;
    }

    /// <summary>
    /// 비활성화 UI Pool에서 UI 검색
    /// </summary>
    /// <param name="key">PopupUI 이름</param>
    /// <returns>비활성화된 Popup UI</returns>
    private PopupUI FindPopUpUIInPool(string key)
    {
        return popupUIPool.GetValueOrDefault(key);
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
            var resource = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/MainCanvas");
            mainCanvas = Instantiate(resource);
        }

        DontDestroyOnLoad(mainCanvas);
    }

    private void InitPopupCanvas()
    {
        popupCanvas = GameObject.Find("PopupCanvas");

        if (popupCanvas == null)
        {
            //TODO: Resources.Load 나중에 Addressable로 바꾸기
            var resource = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/PopupCanvas");
            popupCanvas = Instantiate(resource);
        }
        
        DontDestroyOnLoad(popupCanvas);
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
            var resource = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/Fader");
            fader = Instantiate(resource).GetComponent<CanvasGroup>();
        }

        DontDestroyOnLoad(fader.gameObject);
    }
    
    /// <summary>
    /// Main Canvas에 있는 UI 정리
    /// </summary>
    private void ClearMainCanvas()
    {
        for (var i = 0; i < mainUIList.Count; i++)
        {
            if (mainUIList[i] == null || mainUIList[i].IsDestroy)
            {
                if (mainUIList[i] != null)
                {
                    Destroy(mainUIList[i].gameObject);
                }
                mainUIList.RemoveAt(i);
            }
            else
            {
                mainUIList[i].gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Popup Canvas 정리
    /// </summary>
    private void ClearPopupCanvas()
    {
        curPopupUIStack.Clear();
        popupUIPool.Clear();
        for (var i = 0; i < popupCanvas.transform.childCount; i++)
        {
            Destroy(popupCanvas.transform.GetChild(i).gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearMainCanvas();
        ClearPopupCanvas();
    }
    #endregion
}