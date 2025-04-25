using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    public WeaponController weaponController;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        weaponController = weaponController.GetComponent<WeaponController>();
        if(Input.GetMouseButtonDown(0))
        {
            weaponController.Fire(false);
        }
    }
}
