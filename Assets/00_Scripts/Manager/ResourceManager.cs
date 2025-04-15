using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    private readonly Dictionary<string, object> resourcePools = new(); // 리소스 캐싱용

    /// <summary>
    /// Resources.Load()와 사용법 같음
    /// </summary>
    public T Load<T>(string resourcePath) where T : Object
    {
        if (resourcePools.TryGetValue(resourcePath, out var value))
        {
            // 캐싱된 리소스 반환
            return value as T;
        }

        var resource = Resources.Load<T>(resourcePath);
        if (resource != null)
        {
            // 처음 사용하는 리소스면 캐싱
            resourcePools.Add(resourcePath, resource);
        }

        return resource;
    }

    /// <summary>
    /// Resources.LoadAll()와 사용법 같음
    /// </summary>
    public T[] LoadAll<T>(string resourcePath) where T : Object
    {
        if (resourcePools.TryGetValue(resourcePath, out var value))
        {
            return value as T[];
        }

        var resource = Resources.LoadAll<T>(resourcePath);
        if (resource != null)
        {
            resourcePools.Add(resourcePath, resource);
        }

        return resource;
    }
}