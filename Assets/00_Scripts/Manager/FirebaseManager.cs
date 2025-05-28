using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseManager : SingletonBehaviour<FirebaseManager>
{
    private FirebaseAuth auth; // 로그인, 회원가입에 사용
    private FirebaseUser user; // 인증이 완료된 유저 정보
    
    private FirebaseFirestore db;

    protected override void Awake()
    {
        base.Awake();
        InitializeFirebase();
    }

    private void Update()
    {
        Debug.Log($"로그인됨: {user.UserId}");
    }

    public void SignUp(string email, string password, string nickname)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                var ui = UIManager.Instance.OpenPopupUI<PopupNotice>();
                ui.SetContentText("회원가입 실패", $"{task.Exception.Message}", "닫기", "확인");
                
                // 회원가입 실패 이유 => 이메일이 비정상 / 비밀번호가 너무 간단 / 이미 가입된 이메일 등등..
                Debug.LogError("회원가입 실패");
                return;
            }

            var newUser = task.Result.User;
            Debug.Log("회원가입 완료");
            
            var userData = new Dictionary<string, object>
            {
                { "nickname", nickname },
                { "score", 0 },
                { "gold", 0 }
            };

            db.Collection("users").Document(newUser.UserId).SetAsync(userData);
        });
    }

    public void SignIn(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                var ui = UIManager.Instance.OpenPopupUI<PopupNotice>();
                ui.SetContentText("로그인 실패", $"{task.Exception.Message}", "닫기", "확인");
                
                Debug.LogError("로그인 실패");
                return;
            }

            var newUser = task.Result.User;
            Debug.Log("로그인 완료");
        });
    }

    public void SignOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
    
    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance;

                auth.StateChanged += OnAuthStateChanged;
                OnAuthStateChanged(this, null); // 처음 한 번 호출해서 현재 로그인 상태 체크
                Debug.Log("Firebase 초기화 완료");
            }
            else
            {
                Debug.LogError($"Firebase 초기화 실패: {dependencyStatus}");
            }
        });
    }
    
    private void OnAuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("로그아웃됨");
            }

            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log($"로그인됨: {user.UserId}");
            }
        }
    }
}