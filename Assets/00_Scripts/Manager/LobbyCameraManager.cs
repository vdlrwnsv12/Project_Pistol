using Cinemachine;
using UnityEngine;

public class LobbyCameraManager : SingletonBehaviour<LobbyCameraManager>
{
    [SerializeField] private GameObject cameraPositionTarget;
    [SerializeField] private GameObject cameraLookTarget;
    [SerializeField] private CinemachineVirtualCamera vcam;

    private StartBtn startBtn;

    private Vector3 moveOffset = new Vector3(3f, 0f, 0f);

    private Vector3 initPos;
    private Quaternion initRot;
    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();
    }

    void Start()
    {
        if (cameraPositionTarget != null)
        {
            initPos = cameraPositionTarget.transform.position;
            initRot = cameraPositionTarget.transform.rotation;
        }
    }

    public void MoveCameraPosition(int direction)
    {
        cameraPositionTarget.transform.position += moveOffset * direction;
    }
    public void ResetCamPosition()
    {
        if (vcam != null)
        {
            vcam.Follow = cameraPositionTarget.transform;
            vcam.LookAt = cameraLookTarget.transform;
        }

        cameraPositionTarget.transform.position = initPos;
        cameraPositionTarget.transform.rotation = initRot;

    }
}
