using UnityEngine;

public class StartBtn : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    public LobbyCameraManager lobbyCameraManager;

    public void OffLobbyUI()
    {

        SetLobbyUI(false);
        lobbyCameraManager.MoveCameraPosition(+1);
    }

    public void OnLobbyUI()
    {

        UIManager.Instance.ClosePopUpUI();
        SetLobbyUI(true);

        GameManager.Instance.selectedCharacter = null;
        GameManager.Instance.selectedWeapon = null;

        lobbyCameraManager.ResetCamPosition();
    }
    private void SetLobbyUI(bool isOn)
    {
        canvas.SetActive(isOn);
    }
}
