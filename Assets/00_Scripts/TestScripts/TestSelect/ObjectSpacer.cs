using UnityEngine;
using UnityEngine.UI;

public class ObjectSpacer : MonoBehaviour
{
    public float radius = 5f;                // 원형 배치 반지름
    public float dragSensitivity = 0.5f;     // 드래그 민감도
    public float rotationSmooth = 5f;        // 회전 속도
    public Transform lookTarget;             // 무기들이 바라볼 타겟 (null이면 바라보지 않음)
    public Text infoText;                    // 드래그 중일 때 끄고 다시 켤 텍스트

    private int weaponCount;
    private float targetAngle = 0f;          // 목표 각도
    private float currentAngle = 0f;         // 현재 각도
    private float dragDelta = 0f;            // 마우스 드래그 차이
    private bool isDragging = false;         // 드래그 상태 체크
    private Vector2 lastMousePos;            // 마지막 마우스 위치

    void Start()
    {
        weaponCount = transform.childCount;
        ArrangeWeapons();
    }

    void Update()
    {
        HandleMouseDrag();  // 드래그 처리
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * rotationSmooth);
        ArrangeWeapons(currentAngle);  // 무기 배치

        // 카메라와 가장 가까운 무기 정보를 업데이트
        UpdateClosestWeaponInfo();
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))  // 마우스 클릭 시작
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;

            // 드래그 시작 시 텍스트 끄기
            if (infoText != null)
            {
                infoText.gameObject.SetActive(false);
            }
        }
        else if (Input.GetMouseButtonUp(0))  // 마우스 클릭 종료
        {
            isDragging = false;
            SnapToNearestAngle();  // 마우스를 놓았을 때 가장 가까운 각도로 맞춤

            // 드래그 종료 후 텍스트 다시 켜기
            if (infoText != null)
            {
                infoText.gameObject.SetActive(true);
            }
        }

        if (isDragging)
        {
            Vector2 currentMousePos = Input.mousePosition;
            dragDelta = currentMousePos.x - lastMousePos.x;  // 드래그한 만큼 이동
            lastMousePos = currentMousePos;

            targetAngle += dragDelta * dragSensitivity;  // 드래그 차이에 맞춰 회전
        }
    }

    // 가장 가까운 90도 단위로 맞추기
    void SnapToNearestAngle()
    {
        targetAngle = Mathf.Round(targetAngle / 90f) * 90f;
    }

    void ArrangeWeapons(float angleOffset = 0f)
    {
        for (int i = 0; i < weaponCount; i++)
        {
            Transform child = transform.GetChild(i);
            float angle = ((360f / weaponCount) * i + angleOffset) * Mathf.Deg2Rad;  // 각도 계산

            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            child.localPosition = pos;

            if (lookTarget != null)
                child.LookAt(lookTarget.position);  // 타겟 바라보기
        }
    }

    // 카메라와 가장 가까운 무기 정보를 업데이트
    void UpdateClosestWeaponInfo()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestWeapon = null;

        // 모든 자식 오브젝트(무기들)에 대해 카메라와의 거리를 계산
        for (int i = 0; i < weaponCount; i++)
        {
            Transform weapon = transform.GetChild(i);
            float distance = Vector3.Distance(Camera.main.transform.position, weapon.position);  // 카메라와 무기 간 거리 계산

            // 가장 가까운 무기 찾기
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWeapon = weapon;
            }
        }

        // 가장 가까운 무기의 정보를 출력
        if (closestWeapon != null && infoText != null)
        {
            WeaponSO weaponData = closestWeapon.GetComponent<WeaponStatHandler>().weaponData;
            infoText.text = $"{weaponData.Name}\n" +
                            //$"Description: {weaponData.Description}\n" +
                            $"Damage: {weaponData.DMG}\n" +
                            $"Max Ammo: {weaponData.MaxAmmo}\n"+
                            $"Cost: {weaponData.Cost}$";
        }
    }
}
