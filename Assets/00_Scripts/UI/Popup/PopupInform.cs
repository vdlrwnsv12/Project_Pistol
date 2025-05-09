using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupInform : PopupUI
{
    private ICameraReturnable camReturnTarget;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statusText;
    public Button backButton;
    public Button selectButton;

    public static PopupInform LastInstance {get; private set;}
    private void Awake()
    {
        LastInstance = this;
        backButton.onClick.AddListener(OnBackButtonClicked);
        selectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    public void SetCharacterInfo(CharacterSO data)
    {
        Debug.Log($"SetCharacterInfo 호출됨: {data.Name}");
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        statusText.text = $"Status\n" +
                          $"RCL: {data.RCL.ToString().PadRight(5)} HDL: {data.HDL}\n" +
                          $"STP: {data.STP.ToString().PadRight(5)} SPD: {data.SPD}";
    }

    public void SetCamReturnTarget(ICameraReturnable target)
    {
        camReturnTarget = target;
    }
    private void OnBackButtonClicked()
    {
        camReturnTarget?.CamReturn();
        UIManager.Instance.ClosePopUpUI();
    }

    private void OnSelectButtonClicked()
    {
        //ToDo: 선택버튼 클릭시 게임매니저에 캐릭터 데이터 보내고 총기 선택으로 카메라 이동
    }

    public override bool IsDestroy { get; set; }
    public override bool IsHideNotFocus { get; protected set; }
}
