using DataDeclaration;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MainUI
{
    public override MainUIType UIType { get; protected set; }

    [SerializeField] private TextMeshProUGUI gameTitle;
    [SerializeField] private Button startBtn;
    [SerializeField] private Button signOutBtn;

    private void Awake()
    {
        gameTitle.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(false);
        signOutBtn.gameObject.SetActive(false);
        
        startBtn.onClick.AddListener(StartGame);
        signOutBtn.onClick.AddListener(SignOut);

        AuthenticationService.Instance.SignedIn += () => ReadyToGameStart(true);
        AuthenticationService.Instance.SignedOut += () => ReadyToGameStart(false);
    }
    
    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(activeUIType == UIType);
    }

    private void ReadyToGameStart(bool isSignedIn)
    {
        gameTitle.gameObject.SetActive(isSignedIn);
        startBtn.gameObject.SetActive(isSignedIn);
        signOutBtn.gameObject.SetActive(isSignedIn);
        if (!isSignedIn)
        {
            UIManager.Instance.OpenPopupUI<PopupSignIn>();
        }
    }

    private void StartGame()
    {
        Debug.Log("로비 씬 전환");
        StartCoroutine(SceneLoadManager.Instance.LoadScene(Scene.Lobby));
    }

    private void SignOut()
    {
        UserManager.Instance.SignOut();
    }
}