using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public GameObject playerObject; // <- 인스펙터에 Player 오브젝트 할당
    public CharacterSO character;
    public GameObject canvas;
    public void SelectPlayer()
    {
        CharacterManager.Instance.SelectCharacter = character;
        Cursor.lockState = CursorLockMode.Locked;   // 커서 숨기기
        Player player = playerObject.GetComponent<Player>();
        player.SetCharacterData(character); 
        canvas.SetActive(false);

     
    }
   
}