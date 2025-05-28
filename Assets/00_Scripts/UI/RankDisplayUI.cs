using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

public class RankDisplayUI : MonoBehaviour
{
    [SerializeField] private Text userName;
    [SerializeField] private Text scoreText;

    public void SetUI(DocumentSnapshot data)
    {
        userName.text = data.GetValue<string>("nickname");
        scoreText.text = data.GetValue<int>("score").ToString();
    }
}
