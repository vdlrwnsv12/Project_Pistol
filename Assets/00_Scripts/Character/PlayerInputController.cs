using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(ForceReceiver))]
public class PlayerInputController : MonoBehaviour
{
    public PlayerInputs playerInputs { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }
  
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;
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
