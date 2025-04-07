using UnityEngine;

public class Player : MonoBehaviour
{
    // Player 입력 and 여러 컴포넌트 관리
  [field:Header("Animations")]
   [field:SerializeField] public PlayerAnimationData AnimationData {  get; private set; }

    public Animator animator;
    public PlayerController Input { get; private set; } 
    public CharacterController Controller { get; private set; }

    private void Awake()
    {
        AnimationData.Initialize();
        animator = GetComponent<Animator>();
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서 숨기기
    }
}
