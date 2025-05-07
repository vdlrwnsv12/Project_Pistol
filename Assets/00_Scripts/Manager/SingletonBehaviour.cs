using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Component
{
    protected static bool isDestroyOnLoad = false;
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));
                if (instance == null)
                {
                    SetupInstance();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        RemoveDuplicates();
    }

    private static void SetupInstance()
    {
        var go = new GameObject
        {
            name = typeof(T).Name
        };
        instance = go.AddComponent<T>();
    }

    private void RemoveDuplicates()
    {
        if (instance == null)
        {
            instance = this as T;
            if (!isDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}