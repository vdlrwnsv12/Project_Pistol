using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupAuth : PopupUI
{
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField nameInputField;
    
    [SerializeField] private Button signUpOrCloseBtn;
    [SerializeField] private Button signInOrUpBtn;

    private void Awake()
    {
        InitInputField();
        
        signUpOrCloseBtn.onClick.AddListener(OnClickSignInButton);
        signInOrUpBtn.onClick.AddListener(OnClickSignUpButton);
    }
    
    private void InitInputField()
    {
        // ID inputField 초기화
        idInputField.characterLimit = 20;
        idInputField.onEndEdit.AddListener(ValidateString.ValidateID);
        
        // 비밀번호 inputField 초기화
        passwordInputField.characterLimit = 30;
        passwordInputField.onEndEdit.AddListener(ValidateString.ValidatePassword);
        
        nameInputField.gameObject.SetActive(false);
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
