using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 풀링할 프리펩에 PoolableResource 붙이기
/// 오브젝트 가져오기 예)ObjectPoolManager.Instance.GetObjectInPool(prefab.GetComponent<PoolableResource>(), 생성 위치, 생성 회전);
/// 오브젝트 반환 예) ObjectPoolManager.Instance.ReturnToPool(사용이 끝난 오브젝트); or ''.Instance.AutoReturnToPool(사용이 끝난 오브젝트); 
/// CreateNewPool()로 미리 풀 생성 가능.
/// </summary>
public sealed class ObjectPoolManager : SingletonBehaviour<ObjectPoolManager>
{
    private class PoolData
    {
        public Queue<GameObject> PoolQueue;
        public int MaxSize;
        public PoolableResource Prefab;
    }

    private Dictionary<string, PoolData> poolDictionary = new Dictionary<string, PoolData>();

    public GameObject GetObjectInPool(PoolableResource prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab.poolKey))
        {
            CreateNewPool(prefab, 5, 30);
        }

        var poolData = poolDictionary[prefab.poolKey];
        var pool = poolData.PoolQueue;

        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            if (poolData.PoolQueue.Count + 1 <= poolData.MaxSize)
            {
                obj = Instantiate(poolData.Prefab.gameObject);
                obj.GetComponent<PoolableResource>().poolKey = prefab.poolKey; // 복제본도 키 설정
            }
            else
            {
                Debug.LogWarning($"Pool for {prefab.poolKey} has reached max size ({poolData.MaxSize}).");
                return null;
            }
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        var poolable = obj.GetComponent<PoolableResource>();
        if (poolable == null)
        {
            Destroy(obj);
            return;
        }

        if (!poolDictionary.ContainsKey(poolable.poolKey))
        {
            Destroy(obj);
            Debug.LogWarning($"No pool found for {poolable.poolKey}. Object destroyed.");
            return;
        }

        obj.SetActive(false);
        poolDictionary[poolable.poolKey].PoolQueue.Enqueue(obj);
    }

    public void AutoReturnToPool(GameObject obj, float returnTime)
    {
        StartCoroutine(ReturnAfterSeconds(obj, returnTime));
    }

    private IEnumerator ReturnAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ReturnToPool(obj);
    }

    public void CreateNewPool(PoolableResource prefab, int initialSize, int maxSize)
    {
        if (poolDictionary.ContainsKey(prefab.poolKey))
            return;

        var newPool = new PoolData
        {
            PoolQueue = new Queue<GameObject>(),
            MaxSize = maxSize,
            Prefab = prefab
        };

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab.gameObject);
            obj.SetActive(false);
            obj.GetComponent<PoolableResource>().poolKey = prefab.poolKey;
            newPool.PoolQueue.Enqueue(obj);
        }

        poolDictionary.Add(prefab.poolKey, newPool);
    }
}
