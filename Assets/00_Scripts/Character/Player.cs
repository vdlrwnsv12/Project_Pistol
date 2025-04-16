using UnityEngine;

public class Player : MonoBehaviour
{
    // Player 입력 and 여러 컴포넌트 관리
    [field: SerializeField] public CharacterDatas Data { get; private set; }
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator;

    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    private PlayerStatHandler statHandler;

    public ForceReceiver ForceReceiver { get; private set; }
    public FpsCamera FpsCamera { get; private set; }

    public WeaponStatHandler WeaponStatHandler { get; private set; }
    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponent<Animator>();
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        FpsCamera = GetComponent<FpsCamera>();
        stateMachine = new PlayerStateMachine(this);
        statHandler = new PlayerStatHandler(this);
    }
    private void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;   // 커서 숨기기
        stateMachine.ChangeState(stateMachine.IdleState);

     
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void SetWeaponStatHandler(WeaponStatHandler handler)
    {
        WeaponStatHandler = handler;
    }
}
