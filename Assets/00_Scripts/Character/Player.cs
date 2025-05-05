using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    private GameObject weaponPos;

    [Range(0f, 1f)] public float adsSpeedMultiplier = 0.01f;
    [Range(0f, 1f)] public float speedMultiplier = 0.1f;

    #region Properties

    [field: SerializeField] public CharacterSO Data { get; private set; }
    public PlayerStatHandler Stat { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    public Weapon Weapon { get; private set; }

    public CharacterController CharacterController { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerController Controller { get; private set; }

    public PlayerAnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerMotion Motion { get; private set; }
    public TargetSensor TargetSensor { get; private set; }
    private float stepTimer = 0f;
    private float stepInterval = 0.4f; // 0.4초마다 흔든다 (스텝 간격)
    [field: SerializeField] public CinemachineVirtualCamera NonAdsCamera { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera AdsCamera { get; private set; }

    [field: SerializeField] public CinemachineImpulseSource impulseSource;
    #endregion

    private void Awake()
    {
        InitPlayer();

        Animator = GetComponent<Animator>();

        Controller = GetComponent<PlayerController>();
        CharacterController = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Motion = GetComponent<PlayerMotion>();
        TargetSensor = GetComponent<TargetSensor>();
        
        InitWeapon(GameManager.Instance.selectedWeapon.ID);

        InitCamera();
    }

    private void Start()
    {
        StateMachine.ChangeState(StateMachine.IdleState);
    }

    private void Update()
    {
        StateMachine.HandleInput();
        StateMachine.Update();

        if (StateMachine.MovementInput.magnitude > 0)
        {
            Motion.HeadbobUp();
        }
        else
        {
           Motion.HeadbobDown();
        }
        Motion.WeaponShake();
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
        Weapon = Instantiate(resource, weaponPos.transform.position, Quaternion.identity, weaponPos.transform);
    }
}