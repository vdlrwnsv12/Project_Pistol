using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class PopupRankingBoard : PopupUI
{
    [SerializeField] private GameObject contentsPos;
    [SerializeField] private RankDisplayUI contentUI;
    
    private QuerySnapshot snapshot;

    private async void OnEnable()
    {
        try
        {
            await InitAsync();
            SetRank();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task InitAsync()
    {
        snapshot = await FirebaseManager.Instance.DB.Collection("users")
            .OrderBy("score")
            .Limit(3)
            .GetSnapshotAsync();
    }

    private void SetRank()
    {
        try
        {
            foreach (var document in snapshot.Documents)
            {
                var scoreItems = ObjectPoolManager.Instance.GetObject<RankDisplayUI>(contentUI, Vector3.zero, Quaternion.identity);
                scoreItems.transform.SetParent(contentsPos.transform, false);
                    
                scoreItems.SetUI(document);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
