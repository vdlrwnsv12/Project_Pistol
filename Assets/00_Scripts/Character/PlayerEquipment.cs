using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private GameObject currentWeaponObject; // 현재 들고 있는 무기 오브젝트
    public WeaponFireController weaponFireController;
    public WeaponStatHandler weaponStatHandler;
    public Player player;

    [Header("공총 참조")]
    public Transform handTransform; // 무기를 들 위치
    public Transform camRoot;      // 카메라 루트 (조준용 위치 이동에 사용)
    public Camera playerCam;       // 플레이어 카메라 (정조준시 FOV수정에 사용)
    public FpsCamera fpsCamera;    // 커스텀 FPS 카메라 (총 반동 튈 때 사용 제어)
    // public Text bulletStatText;    // 탄약 표시용 UI

    public void SwitchWeapon(WeaponSO weapon)
    {
        if(weapon == null || string.IsNullOrEmpty(weapon.ID))
        {
            Debug.Log("weaponSO없음");
            return;
        }

        if(currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        string path = $"Prefabs/Weapon/{weapon.ID}";
        GameObject weaponPrefab = ResourceManager.Instance.Load<GameObject>(path);
        if(weaponPrefab == null)
        {
            Debug.LogError($"무기 프리팹 로드 실패{path}");
        }
        // 새 무기 생성 및 장착 위치에 붙임
        currentWeaponObject = Instantiate(weaponPrefab, handTransform, false);

        // 무기 스탯 핸들러 세팅
        weaponStatHandler = currentWeaponObject.GetComponent<WeaponStatHandler>();
        if (weaponStatHandler != null)
        {
            weaponStatHandler.SetSharedReferences(handTransform, camRoot, player, playerCam, fpsCamera);

            weaponFireController = currentWeaponObject.GetComponent<WeaponFireController>();
            
            if (weaponFireController != null)
                weaponFireController.InitReferences(); // 발사/재장전/조준 등 기능 초기화
        }
    }
}
