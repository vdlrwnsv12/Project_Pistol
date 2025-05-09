using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public interface ICameraMovable
{
    void CamMove(Vector3 targetPos, Quaternion targetRot);
    void CamReturn();
}
public class SelectPointer : MonoBehaviour, ICameraMovable
{
    //public GameObject popupInformPrefab; // 프리팹 연결해줘야 함
    //public Transform popupParent; // 보통 Canvas 같은 곳 (선택)
    public PlayerInputs inputActions;
    private Vector3 originCamPos;
    private Quaternion originCamRot;
    private CinemachineVirtualCamera vcam;
    public Transform gunTable;

    private SelectSO clickComp;
    private void Awake()
    {
        inputActions = new PlayerInputs();
        inputActions.Enable();
        AddInputActionCallbacks();

    }

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            originCamPos = vcam.transform.position;
            originCamRot = vcam.transform.rotation;
        }
    }

    public void AddInputActionCallbacks()
    {

        inputActions.Camera.ClickCharacter.started += OnCamera;

    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            clickComp = hit.collider.GetComponent<SelectSO>();
            if (clickComp != null)
            {
                GameManager.Instance.selectedCharacter = clickComp.Data;
                Debug.Log($"{clickComp.Data.name} 선택됨!");

                if (hit.collider.CompareTag("Player"))
                {
                    UIManager.Instance.ClosePopUpUI();

                    Transform target = hit.collider.transform;

                    Vector3 cameraPosition = target.position + target.forward * 3f + Vector3.up * 3f;
                    Quaternion cameraRotation = Quaternion.LookRotation(target.position - cameraPosition);

                    //CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
                    if (vcam != null)
                    {
                        vcam.Follow = null;
                        vcam.LookAt = null;

                        // vcam.transform.position = cameraPosition;
                        // vcam.transform.LookAt(target);

                        CamMove(cameraPosition, cameraRotation);

                    }
                }

                //GameObject popupGO = Instantiate(popupInformPrefab, popupParent);

                UIManager.Instance.OpenPopUpUI("PopupInform");
                StartCoroutine(SetPopupInfoDelayed(clickComp.Data));

                Debug.Log("직접 팝업 인스턴스 생성 완료!");
            }
        }
    }

    private IEnumerator SetPopupInfoDelayed(CharacterSO data)
    {
        yield return null;
        PopupInform popup = PopupInform.LastInstance;
        popup.SetCharacterInfo(data);
        popup.SetCamReturnTarget(this);
    }

    public void CamReturn()
    {
        Debug.Log("[SelectPointer] CamReturn 호출됨");
        if (vcam != null)
        {
            vcam.Follow = null;
            vcam.LookAt = null;

            CamMove(originCamPos, originCamRot);

            // vcam.transform.position = originCamPos;
            // vcam.transform.rotation = originCamRot;
        }
    }

    public void CamMove(Vector3 targetPos, Quaternion targetRot)
    {
        StopAllCoroutines();
        StartCoroutine(CamMoveCoroutine(targetPos, targetRot));
    }
    public IEnumerator CamMoveCoroutine(Vector3 targetPos, Quaternion targetRot)
    {
        float duration = 0.8f;
        float elapsed = 0f;

        Vector3 startCamPos = vcam.transform.position;
        Quaternion startCamRot = vcam.transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float easedT = EaseInOutQuad(t);

            vcam.transform.position = Vector3.Lerp(startCamPos, targetPos, easedT);
            vcam.transform.rotation = Quaternion.Slerp(startCamRot, targetRot, easedT);
            yield return null;
        }

        vcam.transform.position = targetPos;
        vcam.transform.rotation = targetRot;
    }

    public void MoveToGunTable()
    {
        if(vcam != null && gunTable != null)
        {
            Vector3 targetPos = gunTable.position + Vector3.up * 3.4f;
            Quaternion targetRot = Quaternion.Euler(80f, 0f, 0f);
            CamMove(targetPos, targetRot);
        }
    }
    private float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }
}