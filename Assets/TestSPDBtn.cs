using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSPDBtn : MonoBehaviour
{
    public ItemReference itemReference;

    public void OnClickUpBtn()
    {
        ItemManager.Instance.ApplyItemStats(itemReference.itemData);
    }
}
