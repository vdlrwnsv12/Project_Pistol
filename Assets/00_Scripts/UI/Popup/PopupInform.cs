using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupInform : PopupUI
{
    private ICameraMovable camMoveTarget;

    private CharacterSO characterSO;
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
        characterSO = data;
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        statusText.text = $"Status\n" +
                          $"RCL: {data.RCL.ToString().PadRight(5)} HDL: {data.HDL}\n" +
                          $"STP: {data.STP.ToString().PadRight(5)} SPD: {data.SPD}";
    }

    public void SetCamReturnTarget(ICameraMovable target)
    {
        camMoveTarget = target;
    }
    private void OnBackButtonClicked()
    {
        camMoveTarget?.CamReturn();
        UIManager.Instance.ClosePopUpUI();
    }

    private void OnSelectButtonClicked()
    {
        //ToDo: 선택버튼 클릭시 게임매니저에 캐릭터 데이터 보내고 총기 선택으로 카메라 이동
        GameManager.Instance.selectedCharacter = characterSO;

        Debug.Log($"{GameManager.Instance.selectedCharacter}선택");

        if(camMoveTarget is SelectPointer pointer)
        {
            pointer.MoveToGunTable();
        }
        UIManager.Instance.ClosePopUpUI();
    }

    public override bool IsDestroy { get; set; }
    public override bool IsHideNotFocus { get; protected set; }
}
