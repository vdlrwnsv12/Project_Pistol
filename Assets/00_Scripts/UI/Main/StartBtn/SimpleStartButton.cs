using UnityEngine;
using UnityEngine.UI;
using DataDeclaration;

public class SimpleStartButton : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Awake()
    {
        if (startButton == null)
        {
            Debug.LogError("StartButton이 연결되지 않았습니다.");
            return;
        }

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OnClickStartGame);
    }

    private void OnClickStartGame()
    {
        Debug.Log("로비 씬 전환");
        SceneLoadManager.Instance.LoadScene(Scene.Lobby);
    }
}
