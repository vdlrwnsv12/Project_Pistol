using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
public class CharacterSelector : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update

    public CharacterSO selectCharacter;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.selectedCharacter = selectCharacter;
        Debug.Log($"{selectCharacter.name} 캐릭터가 선택되었습니다!");
    }
}
