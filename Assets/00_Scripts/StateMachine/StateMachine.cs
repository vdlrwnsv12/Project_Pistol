using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState // 앞으로 모든 상태들은 IState를 상속 받는다
{
    public void Enter();
    public void Exit();
    public void HandleInput(); // 입력받는거 핸들링
    public void Update();
    public void PhysicsUpdate(); // 물리 중력

}
public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState state)
    {
        currentState?.Exit(); // 원래있던 상태 나가기
        currentState = state;
        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update(); 
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate(); 
    }
}
