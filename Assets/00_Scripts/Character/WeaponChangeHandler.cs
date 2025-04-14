using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChangeHandler : MonoBehaviour
{
    public PlayerEquipment playerEquipment;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = PlayerEquipment.Instance.playerCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                WeaponSelector selector = hit.collider.GetComponent<WeaponSelector>();
                if (selector != null)
                {
                    PlayerEquipment.Instance.SwitchWeapon(selector.weaponIndex);
                }
            }
        }
    }
}
