using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Component
{
    private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T) FindAnyObjectByType(typeof(T));
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
            instance = (T)FindAnyObjectByType(typeof(T));
            if (instance == null)
            {
                var go = new GameObject();
                go.name = typeof(T).Name;
                instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }
        }
    
        private void RemoveDuplicates()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
}
