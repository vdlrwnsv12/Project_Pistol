using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class PopupSignUp : PopupUI
{
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField nameInputField;
    
    [SerializeField] private Button signUpBtn;
    [SerializeField] private Button closeBtn;
    
    public override bool IsDestroy { get; set; }
    public override bool IsHideNotFocus { get; protected set; }

    private void Awake()
    {
        IsDestroy = true;
        IsHideNotFocus = true;
        
        InitIDInputField();
        InitPasswordInputField();
        
        signUpBtn.onClick.AddListener(OnClickSignUpButton);
        closeBtn.onClick.AddListener(CloseUI);
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

    private async void OnClickSignUpButton()
    {
        try
        {
            await UserManager.Instance.SignUpWithUsernamePasswordAsync(idInputField.text, passwordInputField.text, nameInputField.text);
            Debug.Log("회원가입 완료");
            UIManager.Instance.ClosePopUpUI();
        }
        catch
        {
            Debug.Log("회원가입 실패");
        }
    }
}