using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataDeclaration;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public sealed class UserManager : SingletonBehaviour<UserManager>
{
    private UserData userData;

    public UserData UserData => userData;

    protected override void Awake()
    {
        base.Awake();
        userData = new UserData();
    }

    private void Update()
    {
        Debug.Log($"접속 토큰: {userData.AccessToken}");
        Debug.Log($"플레이어 ID: {userData.UserID}");
        Debug.Log($"플레이어 이름: {userData.UserName}");

        if (AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("로그인 상태입니다.");
        }
        else
        {
            Debug.Log("아직 로그인하지 않았습니다.");
        }
    }

    /// <summary>
    /// <para>사용자 이름/비밀번호 자격 증명을 사용하여 새 플레이어를 생성한다.</para>
    /// <para>참고: 사용자 이름은 사용자 이름은 대소문자를 구분하지 않습니다. 최소 3자, 최대 20자여야 하며 문자 A-Z 및 a-z, 숫자, 기호 ., -, @, _만 지원한다.</para>
    /// <para>참고: 비밀번호는 대소문자를 구분합니다. 최소 8자, 최대 30자이며 소문자 1자, 대문자 1자, 숫자 1자, 기호 1자 이상을 포함해야 합니다.</para>
    /// </summary>
    /// <param name="userID">사용자 아이디</param>
    /// <param name="password">사용자 비밀번호</param>
    /// <param name="userName">사용자 이름</param>
    public async Task SignUpWithUsernamePasswordAsync(string userID, string password, string userName)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(userID, password);
            userData.AccessToken = AuthenticationService.Instance.AccessToken;
            userData.UserID = AuthenticationService.Instance.PlayerId;
            try
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(userName);
                SaveData(Constants.USER_NAME, userName);
                userData.UserName = AuthenticationService.Instance.PlayerName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

    /// <summary>
    /// <para>사용자 이름/비밀번호 자격 증명을 사용하여 기존 플레이어를 로그인시킨다.</para>
    /// <para>참고: 사용자 이름은 사용자 이름은 대소문자를 구분하지 않습니다. 최소 3자, 최대 20자여야 하며 문자 A-Z 및 a-z, 숫자, 기호 ., -, @, _만 지원한다.</para>
    /// <para>참고: 비밀번호는 대소문자를 구분합니다. 최소 8자, 최대 30자이며 소문자 1자, 대문자 1자, 숫자 1자, 기호 1자 이상을 포함해야 합니다.</para>
    /// </summary>
    /// <param name="userId">사용자 이름</param>
    /// <param name="password">사용자 비밀번호</param>
    public async Task SignInWithUsernamePasswordAsync(string userId, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(userId, password);
            userData.AccessToken = AuthenticationService.Instance.AccessToken;
            userData.UserID = AuthenticationService.Instance.PlayerId;
            await AuthenticationService.Instance.GetPlayerNameAsync();
            userData.UserName = AuthenticationService.Instance.PlayerName;
            //userData.UserName = LoadData(Constants.USER_NAME);
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

    public async void SaveData(string key, object data)
    {
        try
        {
            var saveData = new Dictionary<string, object>{ { key, data } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
            Debug.Log("데이터 저장 성공");
        }
        catch (CloudSaveException e)
        {
            Debug.LogException(e);
            Debug.Log("데이터 저장 실패");
        }
    }

    public async Task<T> LoadData<T>(string key)
    {
        try
        {
            var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{key});
            var data = loadData[key].Value.ToString();
            Debug.Log("데이터 불러오기 성공");
            
            if (typeof(T) == typeof(int))
                return (T)(object)Convert.ToInt32(data);
            if (typeof(T) == typeof(float))
                return (T)(object)Convert.ToSingle(data);
            if (typeof(T) == typeof(bool))
                return (T)(object)Convert.ToBoolean(data);
            if (typeof(T) == typeof(string))
                return (T)(object)data.ToString();
            var json = data.ToString();
            return JsonUtility.FromJson<T>(json);
        }
        catch
        {
            Debug.Log("데이터 불러오기 실패");
            return default;
        }
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignOut(true);
        userData.AccessToken = AuthenticationService.Instance.AccessToken;
        userData.UserID = AuthenticationService.Instance.PlayerId;
        userData.UserName = AuthenticationService.Instance.PlayerName;
    }
}