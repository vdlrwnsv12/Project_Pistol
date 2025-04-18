using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPartButton : MonoBehaviour
{
    public enum AttachmentType
    {
        RedDot,
        Holographic,
        Laser,
        Compensator
    }

    public AttachmentType attachmentType;

    public void Toggle()
    {
        // 씬 안에서 WeaponStatHandler 찾아오기
        WeaponStatHandler handler = FindObjectOfType<WeaponStatHandler>();
        if (handler == null)
        {
            Debug.LogWarning("WeaponStatHandler 못 찾음");
            return;
        }

        // 종류에 따라 토글
        switch (attachmentType)
        {
            case AttachmentType.RedDot:
                ItemManager.Instance.ToggleAttachment(handler.redDot);
                break;
            case AttachmentType.Holographic:
                ItemManager.Instance.ToggleAttachment(handler.holographic);
                break;
            case AttachmentType.Laser:
                ItemManager.Instance.ToggleAttachment(handler.laserPointer);
                break;
            case AttachmentType.Compensator:
                ItemManager.Instance.ToggleAttachment(handler.compensator);
                break;
        }
    }
}

