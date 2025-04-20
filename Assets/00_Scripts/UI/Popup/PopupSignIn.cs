using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataDeclaration;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupSignIn : PopupUI
{
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [SerializeField] private Button signInBtn;
    [SerializeField] private Button signUpBtn;

    protected override void Awake()
    {
        base.Awake();

        InitIDInputField();
        InitPasswordInputField();

        signInBtn.onClick.AddListener(OnClickSignInButton);
        signUpBtn.onClick.AddListener(OnClickSignUnButton);
    }

    private void InitIDInputField()
    {
        idInputField.characterLimit = 20;
        idInputField.onEndEdit.AddListener(ValidateID);
    }

    private void InitPasswordInputField()
    {
        passwordInputField.characterLimit = 30;
        passwordInputField.onEndEdit.AddListener(ValidatePassword);
    }

    private void ValidateID(string input)
    {
        var pattern = @"^[a-zA-Z0-9.@_-]{3,20}$";

        if (Regex.IsMatch(input, pattern))
        {
            Debug.Log("아이디 유효함");
        }
        else
        {
            Debug.Log("아이디 유효하지 않음");
        }
    }

    private void ValidatePassword(string input)
    {
        var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,30}$";

        if (Regex.IsMatch(input, pattern))
        {
            Debug.Log("비밀번호 유효함");
        }
        else
        {
            Debug.Log("비밀번호 유효하지 않음");
        }
    }

    private async void OnClickSignInButton()
    {
        try
        {
            await SignInWithUsernamePasswordAsync(idInputField.text, passwordInputField.text);
            Debug.Log("로그인 성공");
        }
        catch
        {
            Debug.Log("로그인 실패");
        }
    }

    private void OnClickSignUnButton()
    {
        UIManager.Instance.OpenPopupUI<PopupSignUp>();
    }

    /// <summary>
    /// <para>사용자 이름/비밀번호 자격 증명을 사용하여 기존 플레이어를 로그인시킨다.</para>
    /// <para>참고: 사용자 이름은 사용자 이름은 대소문자를 구분하지 않습니다. 최소 3자, 최대 20자여야 하며 문자 A-Z 및 a-z, 숫자, 기호 ., -, @, _만 지원한다.</para>
    /// <para>참고: 비밀번호는 대소문자를 구분합니다. 최소 8자, 최대 30자이며 소문자 1자, 대문자 1자, 숫자 1자, 기호 1자 이상을 포함해야 합니다.</para>
    /// </summary>
    /// <param name="username">사용자 이름</param>
    /// <param name="password">사용자 비밀번호</param>
    private async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
            Debug.Log("인증에 실패했습니다: " + ex.Message);
            throw;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            Debug.Log("요청 실패: " + ex.Message);
            throw;
        }
    }
}