using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupInform : PopupUI
{
    /// <summary>
    /// 캐릭터, 무기 정보를 보여주기 위한 팝업UI클래스
    /// </summary>
    
    //마지막으로 생성된 PopupInform 인스턴스 저장
    public static PopupInform LastInstance { get; private set; }

    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statusText;
    public Button backButton;
    public Button selectButton;

    private ICameraMovable camMoveTarget; //카메라 복귀 대상
    private CharacterSO characterSO; //현재 표시중인 캐릭터 데이터
    private WeaponSO weaponSO; //현재 표시중인 무기데이터


    private void Awake()
    {
        LastInstance = this; //생성된 인스턴스 저장
        //버튼 이벤트 등록
        backButton.onClick.AddListener(OnBackButtonClicked);
        selectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    /// <summary>
    /// 캐릭터 정보를 UI에 설정
    /// </summary>
    public void SetCharacterInfo(CharacterSO data)
    {
        characterSO = data;
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        statusText.text = FormatCharacterStatus(data);
        Debug.Log($"SetCharacterInfo 호출됨: {data.Name}");
    }

    /// <summary>
    /// 무기 정보를 UI에 설정
    /// </summary>
    public void SetWeaponInfo(WeaponSO data)
    {
        weaponSO = data;
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        statusText.text = FormatWeaponStatus(data);
        Debug.Log($"SetGunInfo 호출됨: {data.Name}");
    }

    /// <summary>
    /// 카메라 복귀 대상
    /// </summary>
    public void SetCamReturnTarget(ICameraMovable target)
    {
        camMoveTarget = target;
    }

    /// <summary>
    /// 뒤로가기 버튼 눌렀을때
    /// </summary>
    private void OnBackButtonClicked()
    {
        if (GameManager.Instance.selectedWeapon == null)
        {
            //무기 선택 전이면 카메라 원래 위치로
            camMoveTarget?.CamReturn();
            GameManager.Instance.selectedCharacter = null;
            weaponSO = null;
        }
        else if (camMoveTarget is SelectPointer pointer)
        {
            //무기 선택 후면 카메라 총기 테이블로
            pointer.MoveToGunTable();
            GameManager.Instance.selectedWeapon = null;
            weaponSO = null;
        }

        UIManager.Instance.ClosePopUpUI();
    }

    /// <summary>
    /// 선택 버튼 눌렀을때
    /// </summary>
    private void OnSelectButtonClicked()
    {
        //캐릭터와 무기 정보가 모두 선택됐으면 게임 씬으로 (테스트용 코드라 지워야함) 
        if (GameManager.Instance.selectedCharacter != null && GameManager.Instance.selectedWeapon != null)
        {
            SceneLoadManager.Instance.LoadScene(DataDeclaration.Scene.Stage);//테스트용 코드
            CloseUI();
            return;
        }
        else
        {
            UIManager.Instance.ClosePopUpUI();
        }
        // 아직 무기선택 전이면 총기 테이블로
        if (camMoveTarget is SelectPointer pointer)
        {
            pointer.MoveToGunTable();
            UpdateSelectedData(); //선택 데이터 저장
        }
    }

    /// <summary>
    /// 선택한 데이터 GameManager에 저장
    /// </summary>
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

    /// <summary>
    /// 캐릭터 상태 텍스트 초기화
    /// </summary>
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
