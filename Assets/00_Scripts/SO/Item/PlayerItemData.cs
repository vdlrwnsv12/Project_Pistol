using System;
using DataDeclaration;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/ItemData/PlayerItem")]
public class PlayerItemData : ItemData
{
    [field: SerializeField]public CharacterStat Stat { get; private set; }
}
