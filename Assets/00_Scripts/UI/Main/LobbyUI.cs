using DataDeclaration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LobbyUI : MainUI
{
    public override MainUIType UIType { get; protected set; }
    public override bool IsDestroy { get; set; }

    public string characterText1;
    public string characterText2;
    public string characterText3;
    public Button startBtn;//테스트
    
    private void Awake()
    {
        UIType = MainUIType.Lobby;
        IsDestroy = true;

        startBtn.onClick.AddListener(OnClickStartButton);//테스트
    }
    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(UIType == activeUIType);
    }

    private void OnClickStartButton()//테스트
    {
        SceneManager.LoadScene("02_GameScene");
    }
}
