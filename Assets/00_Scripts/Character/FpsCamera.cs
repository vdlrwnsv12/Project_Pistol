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
    
}