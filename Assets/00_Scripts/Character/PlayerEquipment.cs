using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerEquipment : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // 무기 프리팹 배열 (인덱스로 선택)
    private GameObject currentWeaponObject; // 현재 들고 있는 무기 오브젝트
    private int currentWeaponIndex = -1;

    public Transform handransform; // 무기를 들 위치
    public Transform camRoot;      // 카메라 루트 (조준용 위치 이동에 사용)
    public Camera playerCam;       // 플레이어 카메라
    public FpsCamera fpsCamera;    // 커스텀 FPS 카메라 (흔들림 등 제어)
    public GameObject playerObject;// 플레이어 오브젝트
    public Text bulletStatText;    // 탄약 표시용 UI

    public void SwitchWeapon(int index)
    {
        // 인덱스가 범위 밖이거나 이미 들고 있는 무기라면 리턴
        if (index < 0 || index >= weaponPrefabs.Length || index == currentWeaponIndex)
            return;

        // 기존 무기 제거
        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);

        // 새 무기 생성 및 장착 위치에 붙임
        currentWeaponObject = Instantiate(weaponPrefabs[index], handransform, false);
        currentWeaponIndex = index;

        // 무기 스탯 핸들러 세팅
        var handler = currentWeaponObject.GetComponent<WeaponStatHandler>();
        if (handler != null)
        {
            handler.SetSharedReferences(handransform, camRoot, playerCam, fpsCamera, playerObject, bulletStatText);

            var fireController = currentWeaponObject.GetComponent<WeaponFireController>();
            if (fireController != null)
                fireController.InitReferences(); // 발사/재장전/조준 등 기능 초기화
        }
    }
}
