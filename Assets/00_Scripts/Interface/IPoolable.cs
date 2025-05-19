public interface IPoolable
{
    void OnGetFromPool();      // 풀에서 꺼낼 때 호출
    void OnReturnToPool();     // 풀에 반환될 때 호출
}