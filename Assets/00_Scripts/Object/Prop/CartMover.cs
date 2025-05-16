using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMover : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 1.0f;

    private float t = 0f;

    void Update()
    {
        t += Time.deltaTime * speed;
        float pingPong = Mathf.PingPong(t, 1f);
        float smooth = Mathf.SmoothStep(0f, 1f, pingPong);
        transform.position = Vector3.Lerp(pointA.position, pointB.position, smooth);

    }
}
