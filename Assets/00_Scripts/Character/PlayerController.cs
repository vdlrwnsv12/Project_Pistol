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
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }


}
