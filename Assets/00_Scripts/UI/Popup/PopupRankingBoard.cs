using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;
using Query = Firebase.Database.Query;

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
    
    private void SetRank()
    {
        try
        {
            int idx = 1;
            foreach (var document in snapshot.Documents)
            {
                var scoreItems = ObjectPoolManager.Instance.GetObject<RankDisplayUI>(contentUI, Vector3.zero, Quaternion.identity);
                scoreItems.transform.SetParent(contentsPos.transform, false);
                
                scoreItems.SetUI(idx,document);
                idx++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task InitAsync()
    {
        var query = FirebaseManager.Instance.DB.CollectionGroup("rank").WhereGreaterThan("BestScore", 0).OrderByDescending("BestScore").Limit(20);
        try
        {
            snapshot = await query.GetSnapshotAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
