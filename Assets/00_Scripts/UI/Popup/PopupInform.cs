using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class PopupInform : PopupUI
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statusText;
    public Button backButton;
    public Button selectButton;

    public void SetCharacterInfo(CharacterSO data)
    {
        Debug.Log($"SetCharacterInfo 호출됨: {data.Name}");
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        statusText.text = $"Status\n" +
                          $"RCL: {data.RCL.ToString().PadRight(5)} HDL: {data.HDL}\n" +
                          $"STP: {data.STP.ToString().PadRight(5)} SPD: {data.SPD}";
    }

    public override bool IsDestroy { get; set; }
    public override bool IsHideNotFocus { get; protected set; }
}
