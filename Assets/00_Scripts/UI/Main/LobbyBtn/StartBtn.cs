using UnityEngine;

public class StartBtn : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    

    public void OffLobbyUI()
    {

        SetLobbyUI(false);
        LobbyCameraHandler.Instance.MoveCameraPosition(+1);
    }

    public void OnLobbyUI()
    {

        UIManager.Instance.ClosePopUpUI();
        SetLobbyUI(true);

        GameManager.Instance.selectedCharacter = null;
        GameManager.Instance.selectedWeapon = null;

        LobbyCameraHandler.Instance.ResetCamPosition();
    }
    private void SetLobbyUI(bool isOn)
    {
        canvas.SetActive(isOn);
    }
}
