using System.Collections.Generic;
using DataDeclaration;
using UnityEngine;

//TODO: 구현 중, 사용X
public class ObjectPoolManager : SingletonBehaviour<ObjectPoolManager>
{
    private readonly Dictionary<int, Queue<GameObject>> pools = new();  // key: 리소스 InstanceID  value: IPoolable 상속받은 비활성화된 오브젝트

    /// <summary>
    /// 오브젝트 풀에서 오브젝트 활성화
    /// </summary>
    /// <param name="resource">활성화할 리소스</param>
    /// <param name="position">생성 위치</param>
    /// <param name="rotation">생성 방향</param>
    /// <param name="parent">부모 Transform, 기본: null</param>
    /// <returns></returns>
    public GameObject GetObjectInPool(IPoolable resource, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject newObj;  // 인스턴스화된 게임 오브젝트
        
        var key = resource.ResourceInstanceID;  // 리소스의 고유 InstanceID를 key값으로
        if (pools.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            // Pool에 오브젝트 있을 경우 해당 오브젝트 Dequeue 후 활성화
            newObj = queue.Dequeue();
            newObj.gameObject.SetActive(true);
        }
        else
        {
            // Pool에 오브젝트 없을 경우 resource를 Instantiate
            newObj = Instantiate(resource.GameObject);
        }

        // 자식 오브젝트 여부에 따른 Transform 설정
        if (parent)
        {
            newObj.transform.SetParent(parent);
        }
        newObj.transform.SetPositionAndRotation(position, rotation);
        
        return newObj;
    }
    
    /// <summary>
    /// 오브젝트 풀에 비활성화된 오브젝트 반환
    /// </summary>
    /// <param name="returnObject">비활성화 오브젝트</param>
    public void ReturnObjectToPool(IPoolable returnObject)
    {
        var key = returnObject.ResourceInstanceID;
        if (!pools.TryGetValue(key, out var queue))
        {
            queue = new Queue<GameObject>();
            pools.Add(key, queue);
        }
        queue.Enqueue(returnObject.GameObject);
        returnObject.GameObject.SetActive(false);
        //returnObject.GameObject.transform.SetParent(transform);
    }
}
