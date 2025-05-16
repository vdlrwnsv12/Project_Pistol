using UnityEngine;

[ExecuteAlways]
public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 margin; // XYZ별 margin 조절

    void Update()
    {
        if (target == null) return;

        transform.position = target.position + margin;
    }
}
