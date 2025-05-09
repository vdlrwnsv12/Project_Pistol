using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupInform : PopupUI
{
    public static PopupInform LastInstance { get; private set; }

    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statusText;
    public Button backButton;
    public Button selectButton;

    private ICameraMovable camMoveTarget;
    private CharacterSO characterSO;
    private WeaponSO weaponSO;

    public override bool IsDestroy { get; set; }
    public override bool IsHideNotFocus { get; protected set; }

    private void Awake()
    {
        LastInstance = this;
        backButton.onClick.AddListener(OnBackButtonClicked);
        selectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    public void SetCharacterInfo(CharacterSO data)
    {
        characterSO = data;
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        statusText.text = FormatCharacterStatus(data);
        Debug.Log($"SetCharacterInfo 호출됨: {data.Name}");
    }

    public void SetWeaponInfo(WeaponSO data)
    {
        weaponSO = data;
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        statusText.text = FormatWeaponStatus(data);
        Debug.Log($"SetGunInfo 호출됨: {data.Name}");
    }

    public void SetCamReturnTarget(ICameraMovable target)
    {
        camMoveTarget = target;
    }

    private void OnBackButtonClicked()
    {
        if (GameManager.Instance.selectedWeapon == null)
        {
            camMoveTarget?.CamReturn();
            GameManager.Instance.selectedCharacter = null;
            weaponSO = null;
        }
        else if (camMoveTarget is SelectPointer pointer)
        {
            pointer.MoveToGunTable();
            GameManager.Instance.selectedWeapon = null;
            weaponSO = null;
        }

        UIManager.Instance.ClosePopUpUI();
    }

    private void OnSelectButtonClicked()
    {
        if(GameManager.Instance.selectedCharacter != null && GameManager.Instance.selectedWeapon !=null)
        {
            SceneManager.LoadScene("02_GameScene");//테스트용 코드
            return;
        }
        if (camMoveTarget is SelectPointer pointer)
        {
            pointer.MoveToGunTable();
            UpdateSelectedData();
        }

        UIManager.Instance.ClosePopUpUI();
    }

    private void UpdateSelectedData()
    {
        if (GameManager.Instance.selectedCharacter == null)
        {
            GameManager.Instance.selectedCharacter = characterSO;
        }

        if (GameManager.Instance.selectedWeapon == null)
        {
            GameManager.Instance.selectedWeapon = weaponSO;
        }
    }

    private string FormatCharacterStatus(CharacterSO data)
    {
        return $"Status\n" +
               $"RCL: {data.RCL.ToString().PadRight(5)} HDL: {data.HDL}\n" +
               $"STP: {data.STP.ToString().PadRight(5)} SPD: {data.SPD}";
    }

    private string FormatWeaponStatus(WeaponSO data)
    {
        return $"Status\n" +
               $"Recoil: {data.ShootRecoil.ToString().PadRight(5)} weaponSO: {data.ReloadTime}\n" +
               $"Damage: {data.DMG.ToString().PadRight(5)} MaxAmmo: {data.MaxAmmo}";
    }
}
