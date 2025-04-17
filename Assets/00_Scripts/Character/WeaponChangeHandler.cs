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
                    if (currentWeapon != null)
                    {
                        currentWeapon.SetActive(true);
                    }

                    currentWeapon = selector.gameObject;
                    playerEquipment.SwitchWeapon(selector.weaponIndex);
                    selector.gameObject.SetActive(false);
                }
            }
        }
    }
}
