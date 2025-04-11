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
       
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서 숨기기
        stateMachine.ChangeState(stateMachine.IdleState);
        WeaponStatHandler = GetComponentInChildren<WeaponStatHandler>(true); // 비활성화 포함
        if (WeaponStatHandler == null)
        {
            Debug.LogError("WeaponStatHandler가 할당되지 않았습니다! 계층구조를 확인하세요.");
        }
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
