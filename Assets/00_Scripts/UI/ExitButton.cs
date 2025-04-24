using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void OnClickExitButton()
    {
        GameManager.GameQuit();
    }
}
