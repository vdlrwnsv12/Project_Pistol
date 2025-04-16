using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{

    public CharacterSO character;
    public GameObject canvas;
    public void SelectPlayer()
    {
        CharacterManager.Instance.SelectCharacter = character;
        Cursor.lockState = CursorLockMode.Locked;   // 커서 숨기기
        canvas.SetActive(false);
    }
}