using DataDeclaration;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Controller;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Look.started += OnLookStarted;
        input.playerActions.Attack.started += OnAttack;
        input.playerActions.Reload.started += OnReload;
        input.playerActions.Ads.performed += OnAds;
    }
    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Controller;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Look.canceled -= OnLookStarted;
        input.playerActions.Attack.started -= OnAttack;
        input.playerActions.Reload.started -= OnReload;
        input.playerActions.Ads.performed -= OnAds;
    }

    public virtual void HandleInput() // 입력 값
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
        Move();
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnLookStarted(InputAction.CallbackContext context)
    {
        Vector2 lookDelta = context.ReadValue<Vector2>();
    }

    protected virtual void OnAttack(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnReload(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnAds(InputAction.CallbackContext context)
    {
        stateMachine.IsAds = !stateMachine.IsAds;
        stateMachine.Player.AdsCamera.gameObject.SetActive(stateMachine.IsAds);
        stateMachine.Player.NonAdsCamera.gameObject.SetActive(!stateMachine.IsAds);
    }
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, false);
    }


    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Controller.playerActions.Movement.ReadValue<Vector2>();
        stateMachine.MouseInput = stateMachine.Player.Controller.playerActions.Look.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.CharacterController.Move(((movementDirection * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime);
        
        RotateView();
    }
    
    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.Player.transform.forward;
        Vector3 right = stateMachine.Player.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    private float GetMovementSpeed()
    {
        float baseSpeed = stateMachine.Player.Stat.SPD * stateMachine.Player.speedMultiplier;
        float finalSpeed = baseSpeed;

        if (stateMachine.IsAds)
        {
            finalSpeed *= stateMachine.Player.adsSpeedMultiplier;  // 조준 중일 때 속도 감소
        }
        return finalSpeed;
    }

    private void RotateView()
    {
        float sensitivity = 1f;

        stateMachine.RotationX -= stateMachine.MouseInput.y * sensitivity;
        stateMachine.RotationX = Mathf.Clamp(stateMachine.RotationX, -70f, 70f);
        Quaternion offsetRotation = Quaternion.Euler(-90f, 0f, 0f);
        
        stateMachine.Player.ArmTransform.localRotation = Quaternion.Euler(stateMachine.RotationX, 0, 0) * offsetRotation;
        
        stateMachine.Player.transform.Rotate(Vector3.up * stateMachine.MouseInput.x);
    }
}
