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
                if (hit.collider.CompareTag("Player") && clickComp.characterData != null)
                {
                    GameManager.Instance.selectedCharacter = null;
                    Debug.Log($"{clickComp.characterData.name} 선택됨!");
                    SelectCharactor(hit, clickComp.characterData);
                }
                else if (hit.collider.CompareTag("Gun") && clickComp.weaponData != null)
                {
                    GameManager.Instance.selectedWeapon = clickComp.weaponData;
                    Debug.Log($"{clickComp.weaponData.name} 선택됨!");
                    SelectGun(hit, clickComp.weaponData);
                }
            }
        }
    }
    #region 캐릭터 선택관련
    private void SelectCharactor(RaycastHit hit, CharacterSO data)
    {
        UIManager.Instance.ClosePopUpUI();

        UIManager.Instance.OpenPopUpUI("PopupInform");


        Debug.Log("직접 팝업 인스턴스 생성 완료!");

        Transform target = hit.collider.transform;

        Vector3 cameraPosition = target.position + target.forward * 3f + Vector3.up * 3f;
        Quaternion cameraRotation = Quaternion.LookRotation(target.position - cameraPosition);

        if (vcam != null)
        {
            vcam.Follow = null;
            vcam.LookAt = null;

            CamMove(cameraPosition, cameraRotation);
        }

        StartCoroutine(SetPopupCharacterInfoDelayed(data));
    }

    private IEnumerator SetPopupCharacterInfoDelayed(CharacterSO data)
    {
        yield return null;
        PopupInform popup = PopupInform.LastInstance;
        popup.SetCharacterInfo(data);
        popup.SetCamReturnTarget(this);
    }

    #endregion

    #region 총 선택 관련
    private IEnumerator SetPopupWeaponInfoDelayed(WeaponSO data)
    {
        yield return null;
        PopupInform popup = PopupInform.LastInstance;
        popup.SetWeaponInfo(data);
        popup.SetCamReturnTarget(this);
    }

    private void SelectGun(RaycastHit hit, WeaponSO data)
    {
        if (GameManager.Instance.selectedCharacter == null)
        {
            Debug.LogWarning("캐릭터가 선택되지 않았습니다. 총을 선택할 수 없습니다.");
            return;
        }

        UIManager.Instance.ClosePopUpUI();
        //GameManager.Instance.selectedWeapon = data;
        UIManager.Instance.OpenPopUpUI("PopupInform");
        

        Debug.Log("직접 팝업 인스턴스 생성 완료!");

        Transform target = hit.collider.transform;

        Vector3 offset = new Vector3(0f, 1.5f, 0f); // 위에서 내려다보기
        Vector3 cameraPosition = target.position + offset;
        Quaternion cameraRotation = Quaternion.Euler(90f, 0f, 0f); // 완전 수직 아래를 바라봄

        if (vcam != null)
        {
            vcam.Follow = null;
            vcam.LookAt = null;

            CamMove(cameraPosition, cameraRotation);
            StartCoroutine(SetPopupWeaponInfoDelayed(data));
        }
    }
    #endregion

    #region 카메라 이동관련
    public void CamReturn()
    {
        Debug.Log("[SelectPointer] CamReturn 호출됨");
        if (vcam != null)
        {
            vcam.Follow = null;
            vcam.LookAt = null;

            CamMove(originCamPos, originCamRot);
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
        if (vcam != null && gunTable != null)
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
    #endregion
}