using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInputs { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }

    private FpsCamera fpsCamera;


    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;

        fpsCamera = GetComponent<FpsCamera>();  

     
       
    }

    private void Update()
    {
        UpdateRotate();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        fpsCamera.UpdateRotate(mouseX, mouseY); 
        
    }
}
