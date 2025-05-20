using UnityEngine;

public class StartBtn : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    

    public void OffLobbyUI()
    {

        SetLobbyUI(false);
        LobbyCameraManager.Instance.MoveCameraPosition(+1);
    }

    public void OnLobbyUI()
    {

        UIManager.Instance.ClosePopUpUI();
        SetLobbyUI(true);

        GameManager.Instance.selectedCharacter = null;
        GameManager.Instance.selectedWeapon = null;

        LobbyCameraManager.Instance.ResetCamPosition();
    }
    private void SetLobbyUI(bool isOn)
    {
        canvas.SetActive(isOn);
    }
}
