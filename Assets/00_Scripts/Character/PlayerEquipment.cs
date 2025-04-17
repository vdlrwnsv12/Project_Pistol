using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    public GameObject[] weaponPrefabs;
    private GameObject currentWeaponObject;
    private int currentWeaponIndex = -1;

    [Header("공통 참조")]
    public Transform handransform;
    public Transform camRoot;
    public Camera playerCam;
    public FpsCamera fpsCamera;
    public GameObject playerObject;
    public Text bulletStatText;

    /// <summary>
    /// 무기 교체
    /// 사용법 ex) playerEquipment.SwitchWeapon(selector.weaponIndex);
    /// </summary>
    /// <param name="index"></param>
    public void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weaponPrefabs.Length || index == currentWeaponIndex)
            return;

        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        currentWeaponObject = Instantiate(weaponPrefabs[index], handransform, false);
        currentWeaponIndex = index;

        var handler = currentWeaponObject.GetComponent<WeaponStatHandler>();
        if (handler != null)
        {
            handler.SetSharedReferences(handransform, camRoot, playerCam, fpsCamera, playerObject, bulletStatText);
            // 여기에서 InitReferences 호출
            var fireController = currentWeaponObject.GetComponent<WeaponFireController>();
            if (fireController != null)
            {
                fireController.InitReferences();
            }
        }
    }

}
