using UnityEngine;
using System.Collections.Generic;
using DataDeclaration;
[CreateAssetMenu(fileName = "Character", menuName = "SO/CharacterDatas")]

/// <summary>
/// 접근 방법
/// 캐릭터 매니저를 통해서 각 스탯에 접근 할 수 있음
/// ex) CharacterManager.Instance.SelectCharacter.ID
/// </summary>
public class CharacterDatas : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public float RCL;
    public float HDL;
    public float STP;
    public float SPD;
    public int Cost;
   
}
