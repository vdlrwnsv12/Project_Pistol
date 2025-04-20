using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

/// <summary>
/// Unity Gaming Service 관리 클래스
/// </summary>
public sealed class UGSManager : SingletonBehaviour<UGSManager>
{
    protected override async void Awake()
    {
        try
        {
            base.Awake();

            var options = new InitializationOptions();
            options.SetOption("Authentication.Anonymous", false); // 익명 로그인 비활성화

            // Unity Services SDK 초기화
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// 인증 이벤트 등록
    /// <para>플레이어의 상태에 대한 업데이트를 받으려면, SignedIn, SignInFailed, SignedOut 이벤트 핸들러에 함수를 등록한다.</para>
    /// </summary>
    public static void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => { Debug.LogError(err); };

        AuthenticationService.Instance.SignedOut += () => { Debug.Log("Player signed out."); };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }
}