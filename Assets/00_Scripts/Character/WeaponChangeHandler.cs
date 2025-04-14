using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChangeHandler : MonoBehaviour
{
    public PlayerEquipment playerEquipment;
    [SerializeField] private GameObject currentWeapon;
    void Update()
    {
        Ray ray = PlayerEquipment.Instance.playerCam.ScreenPointToRay(Input.mousePosition);
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
                    PlayerEquipment.Instance.SwitchWeapon(selector.weaponIndex);
                    selector.gameObject.SetActive(false);
                }
            }
        }
    }
}
