using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CinemachineVirtualCamera ads;
    public CinemachineVirtualCamera nonAds;
    
    // Player 입력 and 여러 컴포넌트 관리
    [field: SerializeField] public CharacterSO Data { get; set; }

    [field: Header("Animations"), SerializeField]
    public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }

    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public PlayerStatHandler StatHandler;

    public ForceReceiver ForceReceiver { get; private set; }
    //public FpsCamera FpsCamera { get; private set; }
    public PlayerStateMachine StateMachine;
    
    [Range(0f, 1f)] public float adsSpeedMultiplier = 0.03f;
    [Range(0f, 1f)] public float speedMultiplier = 0.1f;

    [SerializeField] private GameObject weaponPos;
    public GameObject WeaponPos => weaponPos;
    public Weapon Weapon { get; private set; }
    
    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponent<Animator>();
        
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        
        //FpsCamera = GetComponent<FpsCamera>();
        StatHandler = new PlayerStatHandler(this);
        StateMachine = new PlayerStateMachine(this);

        if (weaponPos == null)
        {
            weaponPos = gameObject.transform.Find("WeaponPos").gameObject;
        }
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
    
    public void InitWeapon(string weaponID)
    {
        var resource = ResourceManager.Instance.Load<Weapon>($"Prefabs/Weapon/{weaponID}");
        Weapon = Instantiate(resource, WeaponPos.transform.position, Quaternion.identity, WeaponPos.transform);
    }
}
