using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInputController))]
public class Player : MonoBehaviour
{
    [Range(0f, 1f)] public float adsSpeedMultiplier = 0.03f;
    [Range(0f, 1f)] public float speedMultiplier = 0.1f;

    #region Properties

    public CharacterSO Data { get; private set; }
    public PlayerStatHandler Stat { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    public GameObject WeaponPos { get; private set; }
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
    }

    private void Start()
    {
        StateMachine.ChangeState(StateMachine.IdleState);
    }

    private void Update()
    {
        StateMachine.HandleInput();
        StateMachine.Update();
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

        if (WeaponPos == null)
        {
            WeaponPos = gameObject.transform.FindDeepChildByName("WeaponPos").gameObject;
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
        Weapon = Instantiate(resource, WeaponPos.transform.position, Quaternion.identity, WeaponPos.transform);
    }
}