using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SelectPointer : MonoBehaviour
{
    public GameObject popupInformPrefab; // 프리팹 연결해줘야 함
    public Transform popupParent; // 보통 Canvas 같은 곳 (선택)
    public PlayerInputs inputActions;
    private void Awake()
    {
        inputActions = new PlayerInputs();
        inputActions.Enable();
        AddInputActionCallbacks();
    }

    public void AddInputActionCallbacks()
    {
        inputActions.Camera.ClickCharacter.started += OnCamera;
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Player clickComp = hit.collider.GetComponent<Player>();
            if (clickComp != null)
            {
                GameManager.Instance.selectedCharacter = clickComp.Data;
                Debug.Log($"{clickComp.Data.name} 선택됨!");
          
                GameObject popupGO = Instantiate(popupInformPrefab, popupParent);
                PopupInform popup = popupGO.GetComponent<PopupInform>();
                popup.SetCharacterInfo(clickComp.Data);

                Debug.Log("직접 팝업 인스턴스 생성 완료!");
            }
        }
    }

}