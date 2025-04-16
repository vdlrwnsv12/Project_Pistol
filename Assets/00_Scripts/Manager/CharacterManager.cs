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

    public CharacterSO SelectCharacter;
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

}
