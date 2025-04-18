using UnityEngine;

public class WeaponDataForButton : MonoBehaviour
{
    public WeaponSO WeaponData;

    public void OnClickWeaponButtton()
    {
        Debug.Log($"[버튼 클릭] WeaponData: {WeaponData.name}, ID: {WeaponData.ID}");
        GameManager.Instance.selectedWeapon = WeaponData;
    }
}
