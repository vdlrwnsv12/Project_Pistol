using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    private readonly Dictionary<string, object> resourcePools = new();

    public T Load<T>(string resourcePath) where T : Object
    {
        if (resourcePools.TryGetValue(resourcePath, out var value))
        {
            return value as T;
        }
        var resource = Resources.Load<T>(resourcePath);
        if (resource != null)
        {
            resourcePools.Add(resourcePath, resource);
        }
        return resource;
    }

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
