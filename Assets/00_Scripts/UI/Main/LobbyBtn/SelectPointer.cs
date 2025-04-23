using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SelectPointer : MonoBehaviour
{

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

    public  void OnCamera(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Player clickComp = hit.collider.GetComponent<Player>();
            if (clickComp != null)
            {
                GameManager.Instance.selectedCharacter = clickComp.Data;
                Debug.Log($"{clickComp.Data.name} 선택됨!");
            }
        }
    }
   
}