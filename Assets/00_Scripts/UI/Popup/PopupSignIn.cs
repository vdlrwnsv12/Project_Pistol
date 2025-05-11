using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSignIn : PopupUI
{
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [SerializeField] private Button signInBtn;
    [SerializeField] private Button signUpBtn;
    
    public override bool IsDestroy { get; set; }
    public override bool IsHideNotFocus { get; protected set; }

    private void Awake()
    {
        IsDestroy = true;
        IsHideNotFocus = true;
        
        InitIDInputField();
        InitPasswordInputField();
        
        signInBtn.onClick.AddListener(OnClickSignInButton);
        signUpBtn.onClick.AddListener(OnClickSignUpButton);
    }

    private void InitIDInputField()
    {
        idInputField.characterLimit = 20;
        idInputField.onEndEdit.AddListener(ValidateString.ValidateID);
    }

    private void InitPasswordInputField()
    {
        passwordInputField.characterLimit = 30;
        passwordInputField.onEndEdit.AddListener(ValidateString.ValidatePassword);
    }

    private void OnClickSignUpButton()
    {
        CloseUI();
        UIManager.Instance.OpenPopupUI<PopupSignUp>();
    }
    
    private async void OnClickSignInButton()
    {
        try
        {
            await UserManager.Instance.SignInWithUsernamePasswordAsync(idInputField.text, passwordInputField.text);
            Debug.Log("로그인 성공");
            CloseUI();
        }
        catch
        {
            Debug.Log("로그인 실패");
        }
    }
}