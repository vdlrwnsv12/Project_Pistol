using System.Collections;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    [SerializeField] private float rotCamXAxisSpeed = 1;
    [SerializeField] private float rotCamYAxisSpeed = 1;
    [SerializeField] private float sensitivity = 0.05f; // 초기 감도 값
    private const string SensitivityKey = "MouseSensitivity";

    private float limitMinx = -80;
    private float limitMaxX = 50;

    private float eulerAngleX;
    private float eulerAngleY;
    public bool isADS = false;
    public bool isMoving = false;
    [SerializeField] private float walkBobSpeed = 10f;
    [SerializeField] private float walkBobAmount = 0.05f;

    private float bobTimer;
    private Vector3 initialLocalPosition;

    public float stpValue = 1f; // 외부에서 주입받을 STP 수치

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
        LoadSensitivity(); // OptionUI에서 캐시된 값을 불러와 감도 적용
    }

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed * sensitivity;
        eulerAngleX -= mouseY * rotCamXAxisSpeed * sensitivity;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinx, limitMaxX);
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    public void ApplyRecoil(float recoilAmount)
    {
        StopAllCoroutines();
        StartCoroutine(RecoilCoroutine(recoilAmount));
    }

    IEnumerator RecoilCoroutine(float recoilAmount)
    {
        float duration = 0.075f; // 반동 걸리는 시간
        float elapsed = 0f;

        float startX = eulerAngleX;
        float targetX = eulerAngleX - recoilAmount;
        targetX = ClampAngle(targetX, limitMinx, limitMaxX);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            eulerAngleX = Mathf.Lerp(startX, targetX, t);
            transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
            yield return null;
        }

        eulerAngleX = targetX;
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void UpdateHeadBob(bool isMoving)
    {
        if (isADS || !isMoving)
        {
            bobTimer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPosition, Time.deltaTime * walkBobSpeed);
            return;
        }

        bobTimer += Time.deltaTime * walkBobSpeed;

        float bobAmountAdjusted = walkBobAmount / Mathf.Clamp(stpValue, 0.1f, 999f);
        float newY = initialLocalPosition.y + Mathf.Sin(bobTimer) * bobAmountAdjusted;
        transform.localPosition = new Vector3(initialLocalPosition.x, newY, initialLocalPosition.z);
    }

    // 감도 값 설정
    public void SetSensitivity(float hipValue)
    {
        sensitivity = hipValue * 0.02f;
    }

    // OptionUI에서 캐시된 감도 값을 불러오는 메서드
    private void LoadSensitivity()
    {
        sensitivity = OptionUI.cachedHipSensitivity; // OptionUI에서 가져온 값 사용
        SetSensitivity(sensitivity); // 감도 설정 적용
    }

    public float GetSensitivity()
    {
        return sensitivity;
    }
}
