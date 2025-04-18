using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangeHandler : MonoBehaviour
{
    public PlayerEquipment playerEquipment;
    [SerializeField] private GameObject currentWeapon;
    void Update()
    {
        Ray ray = playerEquipment.playerCam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f) && hit.collider.CompareTag("Gun"))
        {
            WeaponSelector selector = hit.collider.GetComponent<WeaponSelector>();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (selector != null)
                {
                    // 기존 무기 다시 바닥에 보여줌
                    if (currentWeapon != null)
                        currentWeapon.SetActive(true);

                    // 새 무기 들고, 인덱스로 무기 변경
                    currentWeapon = selector.gameObject;
                    playerEquipment.SwitchWeapon(selector.weaponIndex);

                    // 현재 바닥 무기 숨김 (플레이어 손으로 간다고 가정)
                    selector.gameObject.SetActive(false);
                }
            }
        }
    }
}
