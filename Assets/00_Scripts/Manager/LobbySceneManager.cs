using UnityEngine;

public class LobbySceneManager : MonoBehaviour
{
    private static LobbySceneManager instance;

    public static LobbySceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<LobbySceneManager>();
                if (instance == null)
                {
                    var go = new GameObject
                    {
                        name = nameof(LobbySceneManager)
                    };
                    instance = go.AddComponent<LobbySceneManager>();
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
        UIManager.Instance.InitUI<LobbyUI>();
    }
}
