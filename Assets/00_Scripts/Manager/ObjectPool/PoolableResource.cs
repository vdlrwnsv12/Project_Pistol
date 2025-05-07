using UnityEngine;

/// <summary>
/// PoolKey = 프리페별 고유한 이름 설정
/// isAutoReturn 자동 반환 할지 안할지
/// returnTime =  반환 시간
/// </summary>
public class PoolableResource : MonoBehaviour
{
    public string poolKey;  // 풀 키 이름 지정
    public bool isAutoReturn = true;
    public float returnTime = 5f;
}
