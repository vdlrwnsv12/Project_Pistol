using UnityEngine;

public class StartBtn : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform cameraPositionTarget;

    private Vector3 moveOffset = new Vector3(3f, 0f, 0f);

    public void OffLobbyUI()
    {
        SetLobbyUI(false);
        MoveCameraPosition(+1);
    }

    public void OnLobbyUI()
    {
        SetLobbyUI(true);
        MoveCameraPosition(-1);
    }

    private void SetLobbyUI(bool isOn)
    {
        canvas.SetActive(isOn);
    }

    private void MoveCameraPosition(int direction)
    {
        cameraPositionTarget.position += moveOffset * direction;
    }
}
