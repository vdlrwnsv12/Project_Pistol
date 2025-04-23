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
//TODO: 테스트 코드 합치면 삭제해야함
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerEquipment.SwitchWeapon(GameManager.Instance.selectedWeapon);
        }
    }
}