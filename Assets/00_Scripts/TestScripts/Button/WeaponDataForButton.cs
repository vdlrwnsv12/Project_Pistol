using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponDataForButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public WeaponSO WeaponData;
    public GameObject tooltipUI;
    public Text tooltipText;


    public void OnClickWeaponButtton()
    {
        Debug.Log($"[버튼 클릭] WeaponData: {WeaponData.name}, ID: {WeaponData.ID}");
        GameManager.Instance.selectedWeapon = WeaponData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (WeaponData != null && tooltipUI != null && tooltipText != null)
        {
            tooltipUI.SetActive(true);
            tooltipText.text =
                $"<b>{WeaponData.Name}</b>\n" +
                $"ID: {WeaponData.ID}\n" +
                $"설명: {WeaponData.Description}\n" +
                $"공격력: {WeaponData.DMG}\n" +
                $"반동: {WeaponData.ShootRecoil}\n" +
                $"장전 시간: {WeaponData.ReloadTime}s\n" +
                $"최대 탄약: {WeaponData.MaxAmmo}\n" +
                $"가격: {WeaponData.Cost}";
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipUI != null)
            tooltipUI.SetActive(false);
    }
}