using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    private const float SPEED_MULTIPLIER = 0.3f;  // 비율 값
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }
    public virtual void Enter()
    {
        AddInputActionCallbacks();
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;

    }

    public virtual void Exit()
    {
    }

    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Look.started += OnLookStarted;
        input.playerActions.Attack.started += OnAttack;
        input.playerActions.Reload.started += OnReload;
       
    }
    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Look.canceled -= OnLookStarted;
        input.playerActions.Attack.started -= OnAttack;
        input.playerActions.Reload.started -= OnReload;
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
        stateMachine.Player.FpsCamera.UpdateRotate(lookDelta.x, lookDelta.y);
    }

    protected virtual void OnAttack(InputAction.CallbackContext context)
    {
       
    }
   
    protected virtual void OnReload(InputAction.CallbackContext context)
    {
       
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
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }
    // 움직임 로직

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        //CameraRotate();
        Move(movementDirection);
        //Rotate(movementDirection);

    }


    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCamTransform.forward; // 메인 카메라와 캐릭터가 바라보는 방향 같게 만들어줌
        Vector3 right = stateMachine.MainCamTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.Controller.Move(((direction * movementSpeed)+stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime);

    }

    private float GetMovementSpeed()
    {
        float noAimSpeed = stateMachine.MovementSpeed;
        if (stateMachine.Player.WeaponStatHandler != null && stateMachine.Player.WeaponStatHandler.isADS)
        {
            stateMachine.MovementSpeed = CharacterManager.Instance.SelectCharacter.SPD * SPEED_MULTIPLIER -3;
        }
        else
        {
            stateMachine.MovementSpeed =  CharacterManager.Instance.SelectCharacter.SPD * SPEED_MULTIPLIER;
        }
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

  
}
