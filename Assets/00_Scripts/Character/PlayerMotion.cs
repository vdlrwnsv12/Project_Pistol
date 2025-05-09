using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    private Player player;
    [field: SerializeField] public Transform ArmTransform { get; private set; }
    [field: SerializeField] public GameObject HandPos { get; private set; }

    public Transform rootCam;

    [Range(0.0001f, 1f), SerializeField]
    private float amount = 0.005f;
    [Range(1f, 30f), SerializeField]
    private float frequency = 10.0f;
    [Range(10f, 100f), SerializeField]
    private float smooth = 10.0f;

    private float stepTimer = 0f;
    private float stepInterval = 0.4f; // 0.4초마다 흔든다 (스텝 간격)

    #region
    private float eulerAngleX;
    private float eulerAngleY;

    private float limitMinX = -80;
    private float limitMaxX = 50;
    #endregion

    private Quaternion initialLocalRotation;
    private void Awake()
    {
        player = GetComponent<Player>();

    }
    private void Start()
    {
        initialLocalRotation = HandPos.transform.localRotation;
    }


    /// <summary>
    /// 조준 시 캐릭터 HDL 수치에 따른 조준 흔들림 기능
    /// </summary>
    public void WeaponShake()
    {
        var accuracy = Mathf.Clamp01((99f - player.Stat.HDL) / 98f);
        var shakeAmount = accuracy * 7.5f;

        var rotX = (Mathf.PerlinNoise(Time.time * 0.7f, 0f) - 0.5f) * shakeAmount;
        var rotY = (Mathf.PerlinNoise(0f, Time.time * 0.7f) - 0.5f) * shakeAmount * 3f;
        var rotZ = (Mathf.PerlinNoise(Time.time * 0.7f, Time.time * 0.7f) - 0.5f) * shakeAmount;

        var shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
        HandPos.transform.localRotation = initialLocalRotation * shakeRotation;
    }

    public void HeadbobUp()
    {
        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f; // 타이머 초기화

            float t = Mathf.InverseLerp(1f, 99f, player.Stat.STP);
            float force = Mathf.Lerp(1.0f, 0.1f, t);

            player.impulseSource.GenerateImpulse(force * 0.02f); // 한번 흔들어줌
        }
    }

    public void ApplyRecoil()
    {
        float rcl = player.Stat.RCL; 
        float controlFactor = 0.2f + (0.8f * (1f - rcl/99)); //기획서 공식
        float weaponRecoil = player.Weapon.Stat.Recoil; 
        float recoil = weaponRecoil * controlFactor;
        player.stateMachine.RecoilOffsetX -= recoil;
    }
    public void HeadbobDown()
    {
        stepTimer = Mathf.MoveTowards(stepTimer, stepInterval, Time.deltaTime * 2f);
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
