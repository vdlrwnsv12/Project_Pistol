public interface IState // 앞으로 모든 상태들은 IState를 상속 받는다
{
    public void Enter();
    public void Exit();
    public void HandleInput(); // 입력받는거 핸들링
    public void Update();
    public void PhysicsUpdate(); // 물리 중력
}