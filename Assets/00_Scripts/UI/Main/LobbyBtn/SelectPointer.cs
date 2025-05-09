using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class SelectPointer : MonoBehaviour
{
    //public GameObject popupInformPrefab; // 프리팹 연결해줘야 함
    public Transform popupParent; // 보통 Canvas 같은 곳 (선택)
    public PlayerInputs inputActions;
    private void Awake()
    {
        inputActions = new PlayerInputs();
        inputActions.Enable();
        AddInputActionCallbacks();
    }

    void OnEnable()
    {
        
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
            SelectSO clickComp = hit.collider.GetComponent<SelectSO>();
            if (clickComp != null)
            {
                GameManager.Instance.selectedCharacter = clickComp.Data;
                Debug.Log($"{clickComp.Data.name} 선택됨!");

                if (hit.collider.CompareTag("Player"))
                {
                        Transform target = hit.collider.transform;

                        Vector3 cameraPosition = target.position + target.forward * 3f + Vector3.up * 3f;

                    CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
                    if (vcam != null)
                    {
                        vcam.Follow = null;
                        vcam.LookAt = target;

                        vcam.transform.position = cameraPosition;
                        vcam.transform.LookAt(target);
                    }
                }

                //GameObject popupGO = Instantiate(popupInformPrefab, popupParent);
                if(PopupInform.LastInstance != null)
                {
                    UIManager.Instance.ClosePopUpUI();
                }
                UIManager.Instance.OpenPopUpUI("PopupInform");
                PopupInform popup = PopupInform.LastInstance;
                popup.SetCharacterInfo(clickComp.Data);

                Debug.Log("직접 팝업 인스턴스 생성 완료!");
            }
        }
    }

}