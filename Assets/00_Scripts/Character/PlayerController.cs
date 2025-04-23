using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;

    public Transform rootCam;
    [Range(0.0001f, 1f), SerializeField]
    private float amount = 0.005f;
    [Range(1f, 30f), SerializeField]
    private float frequency = 10.0f;
    [Range(10f, 100f), SerializeField]
    private float smooth = 10.0f;
    
    private Quaternion initialLocalRotation;
    
    public PlayerInputs playerInputs { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }
  
    private void Awake()
    {
        player = GetComponent<Player>();
        
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;
    }

    private void Start()
    {
        initialLocalRotation = player.HandPos.transform.localRotation;
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }
    
    /// <summary>
    /// 조준 시 캐릭터 STP 수치에 따른 흔들림 기능
    /// </summary>
    public void StartHeadBob()
    {
        float t = Mathf.InverseLerp(1f, 99f, player.Stat.STP);  // STP가 1일 때 0, 99일 때 1
        
        float inverseEffect = 1f - t;  // 반비례 효과

        Vector3 pos = Vector3.zero;
        pos.z += Mathf.Lerp(pos.z, Mathf.Sin(Time.time * frequency) * amount * inverseEffect, smooth * Time.deltaTime);
        
        rootCam.localPosition = pos;
        Debug.Log(player.ArmTransform.localPosition);
    }
    
    /// <summary>
    /// 조준 시 캐릭터 HDL 수치에 따른 조준 흔들림 기능
    /// </summary>
    public void WeaponShake()
    {
        var accuracy = Mathf.Clamp01((99f - player.Stat.HDL) / 98f);
        var shakeAmount = accuracy * 7.5f;

        var rotX = (Mathf.PerlinNoise(Time.time * 0.7f, 0f) - 0.5f) * shakeAmount;
        var rotY = (Mathf.PerlinNoise(0f, Time.time * 0.7f) - 0.5f) * shakeAmount * 3f;
        var rotZ = (Mathf.PerlinNoise(Time.time * 0.7f, Time.time * 0.7f) - 0.5f) * shakeAmount;

        var shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
        player.HandPos.transform.localRotation = initialLocalRotation * shakeRotation;
    }
}
