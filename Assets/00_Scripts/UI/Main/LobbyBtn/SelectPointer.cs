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
    public Transform gunTable; // 총 올라가 있는 테이블 위치
    private Vector3 originCamPos; // 초기 상태 카메라 위치
    private Quaternion originCamRot; //초기 상태 카메라 회전
    private CinemachineVirtualCamera vcam; //시네머신 카메라
    private SelectSO clickComp; //클릭시 가져올 데이터 

    private void Awake()
    {
        inputActions = new PlayerInputs();
        inputActions.Enable();
        inputActions.Camera.ClickCharacter.started += OnCamera;
    }

    private void Start() // 카메라 초기 위치 회전 저장
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
            if (clickComp == null) return; //클릭한 오브젝트에 SelectSO가 없으면 리턴

            if (hit.collider.CompareTag("Player") && clickComp.characterData != null) 
            {
                HandleCharacterSelection(hit); //캐릭터를 클릭 했을때 호출 할 함수
            }
            else if (hit.collider.CompareTag("Gun") && clickComp.weaponData != null)
            {
                HandleWeaponSelection(hit); //총을 클릭했을때 호출 할 함수
            }
        }
    }

    private void HandleCharacterSelection(RaycastHit hit)
    {
        GameManager.Instance.selectedCharacter = null; //게임매니저 캐릭터 데이터 삭제
        UIManager.Instance.ClosePopUpUI();
        UIManager.Instance.OpenPopUpUI("PopupInform");

        Transform target = hit.collider.transform; 
        Vector3 cameraPosition = target.position + target.forward * 3f + Vector3.up * 3f; //카메라 위치 클릭한 대상 앞쪽으로 이동
        Quaternion cameraRotation = Quaternion.LookRotation(target.position - cameraPosition); //카메라가 target을 바라보도록 회전

        MoveCamera(cameraPosition, cameraRotation); //위 정보를 토대로 카메라 이동 및 회전
        StartCoroutine(SetPopupCharacterInfoDelayed(clickComp.characterData));//UI가 아직 생성되지 않은 상태로 정보 접근 피하기 위해서 한프레임 대기
    }

    private IEnumerator SetPopupCharacterInfoDelayed(CharacterSO data)
    {
        yield return null;
        PopupInform popup = PopupInform.LastInstance; //PopupInform 인스턴스 가져오기
        popup.SetCharacterInfo(data); // 캐릭터 정보 표시
        popup.SetCamReturnTarget(this); //뒤로가기 버튼을 위한 위치지정
    }

    private void HandleWeaponSelection(RaycastHit hit) 
    {
        if (GameManager.Instance.selectedCharacter == null) //캐릭터가 선택되지 않았다면 총기 선택 x
        {
            Debug.LogWarning("캐릭터가 선택되지 않았습니다. 총을 선택할 수 없습니다.");
            return;
        }
        //캐릭터 선택 버튼을 누르기 전까지 총기 선택은 안되게 해야하지만 총기는 선택 버튼을 누르면 게임이 시작 돼야 해서 바로 게임매니저로 데이터 전송
        GameManager.Instance.selectedWeapon = clickComp.weaponData; 

        UIManager.Instance.ClosePopUpUI();
        UIManager.Instance.OpenPopUpUI("PopupInform");

        Transform target = hit.collider.transform; //총기 위치정보
        Vector3 cameraPosition = target.position + new Vector3(0f, 1.5f, 0f); // 카메라를 총기 바로 위로
        Quaternion cameraRotation = Quaternion.Euler(90f, 0f, 0f); // 총기를 바라보게

        MoveCamera(cameraPosition, cameraRotation);//카메라 이동
        StartCoroutine(SetPopupWeaponInfoDelayed(clickComp.weaponData));//UI가 아직 생성되지 않은 상태로 정보 접근 피하기 위해서 한프레임 대기
    }

    private IEnumerator SetPopupWeaponInfoDelayed(WeaponSO data)
    {
        yield return null;
        PopupInform popup = PopupInform.LastInstance;
        popup.SetWeaponInfo(data); //총기 정보 표시
        popup.SetCamReturnTarget(this); //뒤로가기 버튼을 위한 위치지정
    }

    private void MoveCamera(Vector3 position, Quaternion rotation)
    {
        if (vcam == null) return;
        vcam.Follow = null;
        vcam.LookAt = null; //카메라 수동 조작을 위해 null
        CamMove(position, rotation); //위에서 받아온 타겟 위치 정보를 바탕으로 카메라 이동
    }

    public void CamReturn() //카메라 초기 위치로 복귀
    {
        Debug.Log("[SelectPointer] CamReturn 호출됨");
        MoveCamera(originCamPos, originCamRot);
    }

    public void CamMove(Vector3 targetPos, Quaternion targetRot) //위에서 받아온 타겟 위치 정보를 바탕으로 카메라 이동
    {
        StopAllCoroutines(); //이 클래스엔 카메라 이동관련 코루틴 밖에 없어서 코루틴 중 다른 곳으로 이동하려면 모든 코루틴 정지시켜야힘
        StartCoroutine(CamMoveCoroutine(targetPos, targetRot));
    }

    private IEnumerator CamMoveCoroutine(Vector3 targetPos, Quaternion targetRot) //카메라 이동 관련 코루틴
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

    public void MoveToGunTable() //카메라를 총기 테이블로
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
