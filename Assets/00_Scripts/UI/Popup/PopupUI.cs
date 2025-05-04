/// <summary>
/// Base UI를 상속받은 Popup UI의 부모 클래스
/// </summary>
public abstract class PopupUI : BaseUI
{
    public abstract override bool IsDestroy { get; set; }
    public abstract bool IsHideNotFocus { get; protected set; }
    
    /// <summary>
    /// 팝업창 닫기
    /// </summary>
    protected virtual void CloseUI()
    {
        UIManager.Instance.ClosePopUpUI();
    }
}
