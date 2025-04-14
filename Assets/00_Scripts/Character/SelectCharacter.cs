using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{

    public CharacterDatas character;

    public void SelectPlayer()
    {
        CharacterManager.Instance.SelectCharacter = character;
        
    }
}