public abstract class BaseScene
{
    public abstract void EnterScene();

    public virtual void ExitScene()
    {
        UIManager.Instance.ClearCanvas();
    }
}