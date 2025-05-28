using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class PopupRankingBoard : PopupUI
{
    [SerializeField] private GameObject contentsPos;
    [SerializeField] private RankDisplayUI contentUI;

    private void OnEnable()
    {
        Init();
    }
    
    private void Init()
    {
        FirebaseManager.Instance.DB.Collection("users")
            .OrderBy("score")
            .Limit(3)
            .GetSnapshotAsync().ContinueWith(task => {
                if (task.IsCanceled || task.IsFaulted) return;

                var snapshot = task.Result;
                foreach (var document in snapshot.Documents)
                {
                    string nickname = document.GetValue<string>("nickname");
                    int score = document.GetValue<int>("score");
                    Debug.Log($"닉네임: {nickname}, 점수: {score}");
                    
                    
                    //var scoreItems = ObjectPoolManager.Instance.GetObject<RankDisplayUI>(contentUI, Vector3.zero, Quaternion.identity);
                    //scoreItems.transform.SetParent(contentsPos.transform, false);
                    
                    //scoreItems.SetUI(document);
                }
            });
    }
}
