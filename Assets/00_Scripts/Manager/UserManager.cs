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
        // 회원가입
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(userID, password);
            // 회원가입 성공 시 유저 정보에 데이터 저장
            userData.AccessToken = AuthenticationService.Instance.AccessToken;
            userData.UserID = AuthenticationService.Instance.PlayerId;

            // 유저 이름 설정
            try
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(userName);

                // Unity Cloud Save를 통해 유저 이름 데이터 저장
                try
                {
                    await SaveData(Constants.USER_NAME, userName);
                }
                catch (CloudSaveException e)
                {
                    Debug.LogException(e);
                    await AuthenticationService.Instance.DeleteAccountAsync();
                    throw;
                }

                userData.UserName = AuthenticationService.Instance.PlayerName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        catch (AuthenticationException)
        {
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
    /// Unity Cloud Save를 통해 유저 데이터 저장
    /// </summary>
    /// <param name="key">저장할 데이터의 key 값</param>
    /// <param name="data">저장할 데이터</param>
    public async Task SaveData(string key, object data)
    {
        var saveData = new Dictionary<string, object> { { key, data } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
    }

    /// <summary>
    /// Unity Cloud Save를 통해 유저 데이터 로드
    /// </summary>
    /// <param name="key">로드할 데이터의 key 값</param>
    /// <typeparam name="T">반환받을 데이터의 타입</typeparam>
    /// <returns>로드된 데이터</returns>
    public async Task<T> LoadData<T>(string key)
    {
        var dataKey = new HashSet<string> { key };
        var loadDataDict = await CloudSaveService.Instance.Data.Player.LoadAsync(dataKey);
        var data = loadDataDict[key].Value.GetAs<T>();
        return data;
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignOut(true);
        userData.AccessToken = AuthenticationService.Instance.AccessToken;
        userData.UserID = AuthenticationService.Instance.PlayerId;
        userData.UserName = AuthenticationService.Instance.PlayerName;
    }

    #region Achievement Enums

    // <summary>
    // 도전과제 조건 종류(조건 비교를 위한 타입)
    // </summary>
    //public enum AchievementConditionType
    //{
    //    /// <summary> 누적 킬 수 </summary>
    //    KillCount,

    //    /// <summary> 헤드샷 명중률 (퍼센트) </summary>
    //    HeadshotRatio,

    //    /// <summary> 특정 거리 이상에서 명중 </summary>
    //    DistanceShot,

    //    /// <summary> 연속 명중 횟수 </summary>
    //    ConsecutiveHits,

    //    /// <summary> 한 번도 빗나가지 않고 클리어 </summary>
    //    NoMissRun,

    //    /// <summary> 제한 시간 내 클리어 </summary>
    //    ClearTime,

    //    /// <summary> 연속 콤보 수 </summary>
    //    ComboCount,

    //    /// <summary> 특정 스테이지 클리어 </summary>
    //    StageClear,

    //    /// <summary> 특정 무기로 킬 </summary>
    //    WeaponSpecificKill
    //}

    #endregion
}