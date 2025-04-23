using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInputController))]
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

    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerInputController Input { get; private set; }

    public PlayerAnimationData AnimationData { get; private set; }
    public Animator Animator { get; private set; }

    [field: SerializeField] public CinemachineVirtualCamera NonAdsCamera { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera AdsCamera { get; private set; }
    
    [field: SerializeField] public Transform ArmTransform { get; private set; }

    #endregion

    private void Awake()
    {
        InitPlayer();

        Animator = GetComponent<Animator>();

        Input = GetComponent<PlayerInputController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();

        InitCamera();
        
        initialLocalRotation = HandPos.transform.localRotation;
    }

    private void Start()
    {
        StateMachine.ChangeState(StateMachine.IdleState);
    }

    private void Update()
    {
        StateMachine.HandleInput();
        StateMachine.Update();

        if (StateMachine.IsAds)
        {
            WeaponShake();
        }
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
    }

    public void InitWeapon(string weaponID)
    {
        var resource = ResourceManager.Instance.Load<Weapon>($"Prefabs/Weapon/{weaponID}");
        Weapon = Instantiate(resource, weaponPos.transform.position, Quaternion.identity, weaponPos.transform);
    }
    
    //TODO: 나중에 위치 변경
    private Quaternion initialLocalRotation;
    [field: SerializeField] public GameObject HandPos { get; private set; }
    /// <summary>
    /// 조준 시 캐릭터 HDL 수치에 따른 조준 흔들림 기능
    /// </summary>
    private void WeaponShake()
    {
        var accuracy = Mathf.Clamp01((99f - Stat.HDL) / 98f);
        var shakeAmount = accuracy * 7.5f;

        var rotX = (Mathf.PerlinNoise(Time.time * 0.7f, 0f) - 0.5f) * shakeAmount;
        var rotY = (Mathf.PerlinNoise(0f, Time.time * 0.7f) - 0.5f) * shakeAmount * 3f;
        var rotZ = (Mathf.PerlinNoise(Time.time * 0.7f, Time.time * 0.7f) - 0.5f) * shakeAmount;

        var shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
        HandPos.transform.localRotation = initialLocalRotation * shakeRotation;
    }
}