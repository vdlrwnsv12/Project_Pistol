using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [Header("흔들림 세기")]
    [SerializeField] private float positionShake = 0.1f;
    [SerializeField] private float rotationShake = 0.1f;

    [Header("흔들림 속도")]
    [SerializeField] private float frequency = 1f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float seed;

    void OnEnable()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        seed = Random.Range(0f, 100f);
    }

    void Update()
    {
        float time = Time.time * frequency;

        float offsetX = (Mathf.PerlinNoise(seed, time) - 0.5f) * 2f * positionShake;
        float offsetY = (Mathf.PerlinNoise(seed + 1, time) - 0.5f) * 2f * positionShake;
        float offsetZ = (Mathf.PerlinNoise(seed + 2, time) - 0.5f) * 2f * positionShake;

        float rotX = Mathf.Sin(time * 1.3f) * rotationShake;
        float rotZ = Mathf.Cos(time * 1.5f) * rotationShake;

        transform.position = initialPosition + new Vector3(offsetX, offsetY, offsetZ);
        transform.rotation = initialRotation * Quaternion.Euler(rotX, 0, rotZ);
    }

}
