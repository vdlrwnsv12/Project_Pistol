using UnityEngine;

public class CoroutineRunner : SingletonBehaviour<CoroutineRunner>
{
    private static CoroutineRunner instance;
    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("CoroutineRunner");
                instance = obj.AddComponent<CoroutineRunner>();
                Object.DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
}
