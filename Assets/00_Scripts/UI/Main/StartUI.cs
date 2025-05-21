using UnityEngine;
using UnityEngine.UI;
using DataDeclaration;

public class StartUI : MainUI
{
    public override MainUIType UIType { get; protected set; }
    
    [SerializeField] private Button startBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        Debug.Log("▶ 로비 씬 전환");
        SceneLoadManager.Instance.LoadScene(Scene.Lobby);
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(activeUIType == UIType);
    }
}
