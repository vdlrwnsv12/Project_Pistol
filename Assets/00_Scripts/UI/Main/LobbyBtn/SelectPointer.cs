using Cinemachine;
using System.Collections;
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
    public Transform gunTable;

    private Vector3 originCamPos;
    private Quaternion originCamRot;
    private CinemachineVirtualCamera vcam;
    private SelectSO clickComp;

    private void Awake()
    {
        inputActions = new PlayerInputs();
        inputActions.Enable();
        inputActions.Camera.ClickCharacter.started += OnCamera;
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

    private void OnCamera(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            clickComp = hit.collider.GetComponent<SelectSO>();
            if (clickComp == null) return;

            if (hit.collider.CompareTag("Player") && clickComp.characterData != null)
            {
                HandleCharacterSelection(hit);
            }
            else if (hit.collider.CompareTag("Gun") && clickComp.weaponData != null)
            {
                HandleWeaponSelection(hit);
            }
        }
    }

    private void HandleCharacterSelection(RaycastHit hit)
    {
        GameManager.Instance.selectedCharacter = null;
        UIManager.Instance.ClosePopUpUI();
        UIManager.Instance.OpenPopUpUI("PopupInform");

        Transform target = hit.collider.transform;
        Vector3 cameraPosition = target.position + target.forward * 3f + Vector3.up * 3f;
        Quaternion cameraRotation = Quaternion.LookRotation(target.position - cameraPosition);

        MoveCamera(cameraPosition, cameraRotation);
        StartCoroutine(SetPopupCharacterInfoDelayed(clickComp.characterData));
    }

    private IEnumerator SetPopupCharacterInfoDelayed(CharacterSO data)
    {
        yield return null;
        PopupInform popup = PopupInform.LastInstance;
        popup.SetCharacterInfo(data);
        popup.SetCamReturnTarget(this);
    }

    private void HandleWeaponSelection(RaycastHit hit)
    {
        if (GameManager.Instance.selectedCharacter == null)
        {
            Debug.LogWarning("캐릭터가 선택되지 않았습니다. 총을 선택할 수 없습니다.");
            return;
        }

        GameManager.Instance.selectedWeapon = clickComp.weaponData;
        UIManager.Instance.ClosePopUpUI();
        UIManager.Instance.OpenPopUpUI("PopupInform");

        Transform target = hit.collider.transform;
        Vector3 cameraPosition = target.position + new Vector3(0f, 1.5f, 0f);
        Quaternion cameraRotation = Quaternion.Euler(90f, 0f, 0f);

        MoveCamera(cameraPosition, cameraRotation);
        StartCoroutine(SetPopupWeaponInfoDelayed(clickComp.weaponData));
    }

    private IEnumerator SetPopupWeaponInfoDelayed(WeaponSO data)
    {
        yield return null;
        PopupInform popup = PopupInform.LastInstance;
        popup.SetWeaponInfo(data);
        popup.SetCamReturnTarget(this);
    }

    private void MoveCamera(Vector3 position, Quaternion rotation)
    {
        if (vcam == null) return;
        vcam.Follow = null;
        vcam.LookAt = null;
        CamMove(position, rotation);
    }

    public void CamReturn()
    {
        Debug.Log("[SelectPointer] CamReturn 호출됨");
        MoveCamera(originCamPos, originCamRot);
    }

    public void CamMove(Vector3 targetPos, Quaternion targetRot)
    {
        StopAllCoroutines();
        StartCoroutine(CamMoveCoroutine(targetPos, targetRot));
    }

    private IEnumerator CamMoveCoroutine(Vector3 targetPos, Quaternion targetRot)
    {
        float duration = 0.8f;
        float elapsed = 0f;
        Vector3 startCamPos = vcam.transform.position;
        Quaternion startCamRot = vcam.transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = EaseInOutQuad(Mathf.Clamp01(elapsed / duration));
            vcam.transform.position = Vector3.Lerp(startCamPos, targetPos, t);
            vcam.transform.rotation = Quaternion.Slerp(startCamRot, targetRot, t);
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
}
