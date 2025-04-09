using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float verticalVelocity;
    public Vector3 Movement => Vector3.up * verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y *Time.deltaTime; // 땅에 붙어있으면 유지
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; // 아니면 중력 더해주기
        }
    }
}
