using UnityEngine;
using UnityEngine.UI;
using DataDeclaration;

public class StartUI : MainUI
{
    public override MainUIType UIType { get; protected set; }

    [SerializeField] private Image gameTitleImage;
    [SerializeField] private Button startBtn;

    public void Setup()
    {
        gameTitleImage.gameObject.SetActive(true);
        startBtn.gameObject.SetActive(true);
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        startBtn.onClick.RemoveAllListeners();
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
