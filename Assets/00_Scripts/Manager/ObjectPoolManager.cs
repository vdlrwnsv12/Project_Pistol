using System.Collections.Generic;
using System.Collections;
using DataDeclaration;
using UnityEngine;

/// <summary>
/// 사용법
/// 1. `GetObjectInPool`로 풀에서 오브젝트 꺼내옴
///    - `IPoolable` 인터페이스 있는 애들만 관리됨
///    - 위치 회전 부모 설정 가능
/// 2. `ReturnObjectToPool`로 객체 풀에 반환함
///    - 반환된 객체는 비활성화되고 나중에 재사용됨
/// 3. `AutoReturnToPool`은 일정 시간 후 자동으로 객체 반환해줌
///    - 정해진 시간 지나면 자동 반환됨
///
/// 예시
/// 1. 객체 꺼내는 법
///    GameObject pooledObject = ObjectPoolManager.Instance.GetObjectInPool(resource position rotation);
///
/// 2. 객체 반환하는 법
///    ObjectPoolManager.Instance.ReturnObjectToPool(pooledObject);
///    ObjectPoolManager.Instance.AutoReturnToPool(pooledObject, impactPoolable.ReturnTime);
/// </summary>

public sealed class ObjectPoolManager : SingletonBehaviour<ObjectPoolManager>
{
    private readonly Dictionary<int, Queue<GameObject>> pools = new();

    public GameObject GetObjectInPool(IPoolable resource, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject instance;
        var key = resource.ResourceInstanceID;

        if (pools.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            instance = queue.Dequeue();
        }
        else
        {
            instance = Instantiate(resource.GameObject);
        }

        instance.transform.SetParent(parent ? parent : null);
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.SetActive(true);

        // 인스턴스에서 Poolable 컴포넌트 꺼내서 적용
        var poolable = instance.GetComponent<IPoolable>();
        if (poolable != null && poolable.IsAutoReturn)
        {
            StartCoroutine(AutoReturn(instance, poolable.ReturnTime));
        }

        return instance;
    }

    private IEnumerator AutoReturn(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null && obj.activeInHierarchy)
        {
            ReturnObjectToPool(obj);
        }
    }

    public void ReturnObjectToPool(GameObject instance)
    {
        var poolable = instance.GetComponent<IPoolable>();
        if (poolable == null)
        {
            Debug.LogWarning($"[ObjectPoolManager] 반환 실패: IPoolable이 없음");
            return;
        }

        int key = poolable.ResourceInstanceID;
        if (!pools.TryGetValue(key, out var queue))
        {
            queue = new Queue<GameObject>();
            pools[key] = queue;
        }

        instance.SetActive(false);
        instance.transform.SetParent(transform);
        queue.Enqueue(instance);
    }

    public void AutoReturnToPool(GameObject obj, float delay)
    {
        StartCoroutine(AutoReturn(obj, delay));
    }
}
