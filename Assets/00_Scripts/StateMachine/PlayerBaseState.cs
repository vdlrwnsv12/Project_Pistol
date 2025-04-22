using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    // protected readonly PlayerGroundData groundData;
   

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
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Look.started += OnLookStarted;
        input.playerActions.Attack.started += OnAttack;
        input.playerActions.Reload.started += OnReload;
        input.playerActions.Ads.performed += OnAds;
    }
    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
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
    // HandleAds Coroutine or 
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
       // TO DO HandleAds
    protected virtual void OnAds(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.weaponFireController != null)
        {
            stateMachine.Player.weaponFireController.HandleADS();
        }
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
        stateMachine.Player.Controller.Move(((direction * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        float baseSpeed = stateMachine.Player.StatHandler.Stat.SPD * stateMachine.Player.speedMultiplier;
        float finalSpeed = baseSpeed;

        if (stateMachine.Player.PlayerEquipment.weaponStatHandler != null && stateMachine.Player.PlayerEquipment.weaponStatHandler.isADS)
        {
            finalSpeed *= stateMachine.Player.adsSpeedMultiplier;  // 조준 중일 때 속도 감소
        }
        return finalSpeed;
    }
}
