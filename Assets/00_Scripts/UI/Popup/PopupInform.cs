using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening.Core.Easing;

public class PopupInform : PopupUI
{
    private ICameraMovable camMoveTarget;

    private CharacterSO characterSO;
    private WeaponSO weaponSO;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statusText;
    public Button backButton;
    public Button selectButton;

    public static PopupInform LastInstance { get; private set; }
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

    public void SetWeaponInfo(WeaponSO data)
    {
        Debug.Log($"SetGunInfo 호출됨: {data.Name}");
        weaponSO = data;
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        statusText.text = $"Status\n" +
                          $"Recoil: {data.ShootRecoil.ToString().PadRight(5)} weaponSO: {data.ReloadTime}\n" +
                          $"Damage: {data.DMG.ToString().PadRight(5)} MaxAmmo: {data.MaxAmmo}";
    }

    public void SetCamReturnTarget(ICameraMovable target)
    {
        camMoveTarget = target;
    }
    private void OnBackButtonClicked()
    {
        //ToDo: GameManager에 캐릭터 데이터만 있는 상태로 이 버튼을 누르면 초기 카메라위치로
        // 총기 데이터도 있는 상태로 이 버튼을 누르면 MoveToGunTable호출
        if (GameManager.Instance.selectedWeapon == null)
        {
            camMoveTarget?.CamReturn();
            GameManager.Instance.selectedCharacter = null;
        }
        else
        {
            if (camMoveTarget is SelectPointer pointer)
            {
                pointer.MoveToGunTable();
                GameManager.Instance.selectedWeapon = null;
            }
        }
        UIManager.Instance.ClosePopUpUI();
    }

    private void OnSelectButtonClicked()
    {
        if (camMoveTarget is SelectPointer pointer)
        {
            pointer.MoveToGunTable();

            if (GameManager.Instance.selectedCharacter == null)
            {
                GameManager.Instance.selectedCharacter = characterSO;
                GameManager.Instance.selectedWeapon = null;
            }

            if (GameManager.Instance.selectedWeapon == null)
            {
                GameManager.Instance.selectedWeapon = weaponSO;
            }
        }
        UIManager.Instance.ClosePopUpUI();
    }

    public override bool IsDestroy { get; set; }
    public override bool IsHideNotFocus { get; protected set; }
}
