using DataDeclaration;

/// <summary>
/// Base UI를 상속받은 Main UI의 부모 클래스
/// </summary>
public abstract class MainUI : BaseUI
{
    protected MainUIType uiType;
    
    /// <summary>
    /// Main UI 활성화
    /// </summary>
    /// <param name="activeUIType">활성화 할 Main UI 종류</param>
    public abstract void SetActiveUI(MainUIType activeUIType);
}
