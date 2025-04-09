using UnityEngine;

public class Player : MonoBehaviour
{
    // Player 입력 and 여러 컴포넌트 관리
    [field: SerializeField] public PlayerData Data { get; private set; }
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator;
    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }

    public ForceReceiver ForceReceiver { get; private set; }
    public FpsCamera FpsCamera { get; private set; }
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
       
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서 숨기기
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
}
