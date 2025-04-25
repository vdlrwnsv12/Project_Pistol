using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    private GameObject weaponPos;

    [Range(0f, 1f)] public float adsSpeedMultiplier = 0.03f;
    [Range(0f, 1f)] public float speedMultiplier = 0.1f;

    #region Properties

    public CharacterSO Data { get; private set; }
    public PlayerStatHandler Stat { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    public Weapon Weapon { get; private set; }

    public CharacterController CharacterController { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerController Controller { get; private set; }

    public PlayerAnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }

    [field: SerializeField] public CinemachineVirtualCamera NonAdsCamera { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera AdsCamera { get; private set; }

    [field: SerializeField] public Transform ArmTransform { get; private set; }
    [field: SerializeField] public GameObject HandPos { get; private set; }

    #endregion

    private void Awake()
    {
        InitPlayer();

        Animator = GetComponent<Animator>();

        Controller = GetComponent<PlayerController>();
        CharacterController = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        
        InitWeapon(GameManager.Instance.selectedWeapon.ID);//테스트용 병합시 삭제

        InitCamera();
    }

    private void Start()
    {
        StateMachine.ChangeState(StateMachine.IdleState);
        AdsCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        StateMachine.HandleInput();
        StateMachine.Update();

        if (StateMachine.MovementInput.magnitude > 0)
        {
            Controller.StartHeadBob();
            Controller.StartHeadBob();
        }
        Controller.WeaponShake();
    }

    private void FixedUpdate()
    {
        StateMachine.PhysicsUpdate();
    }

    private void InitPlayer()
    {
        if (Data == null)
        {
            Data = GameManager.Instance.selectedCharacter;
        }

        if (weaponPos == null)
        {
            weaponPos = gameObject.transform.FindDeepChildByName("WeaponPos").gameObject;
        }

        Stat = new PlayerStatHandler(this);
        StateMachine = new PlayerStateMachine(this);
        AnimationData = new PlayerAnimationData();
    }

    private void InitCamera()
    {
        if (AdsCamera && NonAdsCamera)
        {
            return;
        }

        var virtualCams = GetComponentsInChildren<CinemachineVirtualCamera>();
        if (virtualCams[0].name.Equals("ADSVirtualCam"))
        {
            AdsCamera = virtualCams[0];
            NonAdsCamera = virtualCams[1];
        }
        else
        {
            AdsCamera = virtualCams[1];
            NonAdsCamera = virtualCams[0];
        }
    }

    public void InitWeapon(string weaponID)
    {
        var resource = ResourceManager.Instance.Load<Weapon>($"Prefabs/Weapon/{weaponID}");
        Weapon = Instantiate(resource);
        Weapon.transform.SetParent(weaponPos.transform, false);
    }

    #region 테스트

    private float eulerAngleX;
    private float eulerAngleY;

    private float limitMinX = -80;
    private float limitMaxX = 50;

    public void ApplyRecoil(float recoilAmount)
    {
        StopAllCoroutines();
        StartCoroutine(RecoilCoroutine(recoilAmount));
    }

    private IEnumerator RecoilCoroutine(float recoilAmount)
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
            ArmTransform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
            yield return null;
        }

        eulerAngleX = targetX;
        ArmTransform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    #endregion
}