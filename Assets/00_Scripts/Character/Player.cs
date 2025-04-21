using System.Collections;
using test;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player 입력 and 여러 컴포넌트 관리
    [field: SerializeField] public CharacterSO Data { get; set; }

    [field: Header("Animations"), SerializeField]
    public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }

    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public PlayerStatHandler StatHandler;

    public ForceReceiver ForceReceiver { get; private set; }
    public FpsCamera FpsCamera { get; private set; }
    public PlayerStateMachine stateMachine;

    private HeadBob headBob;
    [Range(0f, 1f)] public float adsSpeedMultiplier = 0.03f;
    [Range(0f, 1f)] public float speedMultiplier = 0.1f;

    public Transform weaponPos;
    public Weapon Weapon { get; private set; }
    
    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponent<Animator>();
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        FpsCamera = GetComponent<FpsCamera>();
        headBob = GetComponentInChildren<HeadBob>();
        headBob = GetComponentInChildren<HeadBob>();
        StatHandler = new PlayerStatHandler(this);
        stateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서 숨기기

        stateMachine.ChangeState(stateMachine.IdleState);

        ItemManager.Instance.playerStatHandler = StatHandler;
        Debug.Log("ItemManager.Instance 할당됨");
    }
    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
        
        if (Weapon.Controller != null)
        {
            //Weapon.Controller.HandleADS();
            Debug.Log("fire");
        }
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
    
    public void InitWeapon(string weaponID)
    {
        var resource = ResourceManager.Instance.Load<Weapon>($"Prefabs/Weapon/{weaponID}");
        Weapon = Instantiate(resource, weaponPos.position, Quaternion.identity, weaponPos);
    }
}
