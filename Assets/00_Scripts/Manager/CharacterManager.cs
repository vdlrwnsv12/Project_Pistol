using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public static  CharacterManager instance;
    public static CharacterManager Instance
    {
        get { return instance; }
    }

    public CharacterDatas SelectCharacter;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (CharacterManager.Instance.SelectCharacter.SPD == null)
        {
            Debug.Log("없음");
        }
        else
        {
            Debug.Log(CharacterManager.Instance.SelectCharacter.SPD);
        }
    }
}
