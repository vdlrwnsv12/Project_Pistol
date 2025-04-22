using System.Collections;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    public Transform rootCam;
    private Player player;
    public Vector3 adsPosition = new Vector3(0.062f, -0.007f, 0f);
    private Vector3 currentCamRootTargetPos;
    private Quaternion currentHandTargetRot;
    private Quaternion initialLocalRotation;
    private Vector3 camRootOriginPos;
    private float targetCamY = 0.165f;
    Camera mainCam;

    [SerializeField] private float rotCamXAxisSpeed = 1;
    [SerializeField] private float rotCamYAxisSpeed = 1;
    [SerializeField] private float sensitivity = 0.1f;

    private float limitMinX = -80;
    private float limitMaxX = 50;

    private float eulerAngleX;
    private float eulerAngleY;

    [SerializeField] private float walkBobSpeed = 10f;
    [SerializeField] private float walkBobAmount = 0.05f;

    private float bobTimer;
    private Vector3 initialLocalPosition;

    public float stpValue = 1f; // 외부에서 주입받을 STP 수치

    private void Awake()
    {
        player = GetComponent<Player>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        // WeaponShake();
        // if (player.StateMachine.IsAds)
        // {
        //     HandleADS();
        // }
    }

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed * sensitivity;
        eulerAngleX -= mouseY * rotCamXAxisSpeed * sensitivity;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
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
        targetX = ClampAngle(targetX, limitMinX, limitMaxX);

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

    private void HandleADS()
    {
        Debug.Log("HandleADS");
        if (!player.Weapon.Controller.isReloading)
        {
            Debug.Log("true false전환");
            player.StateMachine.IsAds = !player.StateMachine.IsAds;
        }

        if (player.StateMachine.IsAds)
        {
            currentCamRootTargetPos = adsPosition;
            currentHandTargetRot = initialLocalRotation;

            // redDot 상태에 따라 타겟 Y 설정
            // targetCamY = (weaponStatHandler.redDot != null && weaponStatHandler.redDot.activeSelf) ? 0.18f : 0.16f;
            // bool isOpticActive = optics.Exists(optics => optics.activeSelf); //조준경이 하나라도 켜져 있으면
            // targetCamY = isOpticActive ? 0.18f : 0.16f;
        }
        else
        {
            currentCamRootTargetPos = camRootOriginPos;
            currentHandTargetRot = initialLocalRotation;

            // 정조준 해제 시 기본값으로 복구
            targetCamY = 0.16f;
        }

        // FOV 보간
        float targetFOV = player.StateMachine.IsAds ? 40f : 60f;
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, targetFOV,
            Time.deltaTime * 10f);

        // 위치/회전 보간
        rootCam.localPosition = Vector3.Lerp(rootCam.localPosition,
            currentCamRootTargetPos, Time.deltaTime * 10f);
        player.WeaponPos.transform.localRotation = Quaternion.Lerp(player.WeaponPos.transform.localRotation,
            currentHandTargetRot, Time.deltaTime * 10f);

        //Y 위치만 따로 부드럽게 보간
        Vector3 camLocalPos = mainCam.transform.localPosition;
        camLocalPos.y = Mathf.Lerp(camLocalPos.y, targetCamY, Time.deltaTime * 10f);
        mainCam.transform.localPosition = camLocalPos;
    }

    private void WeaponShake()
    {
        var accuracy = Mathf.Clamp01((99f - player.StatHandler.Stat.HDL) / 98f);
        var shakeAmount = accuracy * 7.5f;

        var rotX = (Mathf.PerlinNoise(Time.time * 0.7f, 0f) - 0.5f) * shakeAmount;
        var rotY = (Mathf.PerlinNoise(0f, Time.time * 0.7f) - 0.5f) * shakeAmount * 3f;
        var rotZ = (Mathf.PerlinNoise(Time.time * 0.7f, Time.time * 0.7f) - 0.5f) * shakeAmount;

        var shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
        player.WeaponPos.transform.localRotation *= shakeRotation;
    }
}