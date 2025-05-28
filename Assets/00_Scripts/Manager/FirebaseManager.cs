using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public Action<bool> SignInState;
    
    public FirebaseAuth Auth => auth;
    public FirebaseUser User => user;

    protected override void Awake()
    {
        base.Awake();
        InitializeFirebase();
    }
    
    private void Update()
    {
        if (user != null)
        {
            Debug.Log(user.UserId);
            Debug.Log(user.Email);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadData<int>("w");
        }
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

    public async Task SignUpAsync(string email, string password, string nickname)
    {
        try
        {
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            var newUser = result.User;
            
            var userData = new Dictionary<string, object>
            {
                { "nickname", nickname },
                { "score", 0 },
                { "gold", 0 }
            };

            await db.Collection("users").Document(newUser.UserId).SetAsync(userData);
        }
        catch (Exception)
        {
            Debug.LogAssertion("회원가입 실패");
            throw;
        }
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

    public async Task SignInAsync(string email, string password)
    {
        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            var newUser = result.User;
        }
        catch (Exception)
        {
            Debug.LogAssertion("로그인 실패");
            throw;
        }
    }

    public void SignOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }

    public void UpdateData(string field, object value)
    {
        db.Collection("users").Document(user.UserId).UpdateAsync(field, value);
    }

    public T LoadData<T>(string key)
    {
        db.Collection("users")
            .OrderBy("score")
            .Limit(20)
            .GetSnapshotAsync().ContinueWith(task => {
                if (task.IsCanceled || task.IsFaulted) return;

                var snapshot = task.Result;
                foreach (var document in snapshot.Documents)
                {
                    string nickname = document.GetValue<string>("nickname");
                    int score = document.GetValue<int>("score");
                    Debug.Log($"닉네임: {nickname}, 점수: {score}");
                }
            });
        
        
        return default(T);
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
                SignOut();
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
                SignInState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log($"로그인됨: {user.UserId}");
                SignInState?.Invoke(true);
            }
        }
    }
}