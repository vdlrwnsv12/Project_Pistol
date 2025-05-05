public abstract class StateMachine
{
    private IState currentState;

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
