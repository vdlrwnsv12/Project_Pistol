using DataDeclaration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MainUI
{
    public override MainUIType UIType { get; protected set; }
    public override bool IsDestroy { get; protected set; }

    public string characterText1;
    public string characterText2;
    public string characterText3;
    protected override void Awake()
    {
        base.Awake();
        UIType = MainUIType.Lobby;
        IsDestroy = true;
    }
    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(UIType == activeUIType);
    }
}
