using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    private static StartSceneManager instance;

    public static StartSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<StartSceneManager>();
                if (instance == null)
                {
                    var go = new GameObject
                    {
                        name = nameof(StartSceneManager)
                    };
                    instance = go.AddComponent<StartSceneManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UIManager.Instance.InitUI<StartUI>();
    }
}