using DataDeclaration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MainUI
{
    [SerializeField] private Button gameStartBtn;
    [SerializeField] private TextMeshProUGUI gameStartText;
    
    public override MainUIType UIType { get; protected set; }
    public override bool IsDestroy { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        UIType = MainUIType.Start;
        IsDestroy = true;
        
        gameStartBtn.onClick.AddListener(OpenSignInPopupUI);
    }

    private void OpenSignInPopupUI()
    {
        UIManager.Instance.OpenPopupUI<PopupSignIn>();
        gameStartText.gameObject.SetActive(false);
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(UIType == activeUIType);
    }
}
