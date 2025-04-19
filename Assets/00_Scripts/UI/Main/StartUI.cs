using DataDeclaration;

public class StartUI : MainUI
{
    protected override void Awake()
    {
        base.Awake();
        uiType = MainUIType.Start;
    }

    public override void SetActiveUI(MainUIType activeUIType)
    {
        gameObject.SetActive(uiType == activeUIType);
    }
}
