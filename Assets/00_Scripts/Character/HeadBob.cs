using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeadBob : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0.1f, 1f)]
    public float Amount = 1f;

    [Range(1f, 30f)]
    public float Frequency = 10.0f;
    [Range(10f, 100f)]
    public float Smooth = 10.0f;

    public Player Player;
    private PlayerStateMachine stateMachine;
    private void Awake()
    {
        Player = GetComponentInParent<Player>();
        stateMachine = Player?.stateMachine; // PlayerStateMachine에 접근

    }

    // Update is called once per frame
    void Update()
    {
        CheckForHeadbobTrigger();
    }

    void CheckForHeadbobTrigger()
    {
        if (Player == null || Player.stateMachine == null) return;

        Vector2 movementInput = Player.stateMachine.MovementInput;
        float headMagnitude = movementInput.magnitude;
        //Debug.Log("no if 문 InputMagnitude" + headMagnitude);
        if (headMagnitude > 0)
        {
            StartHeadBob();
            Debug.Log("InputMagnitude" + headMagnitude);
        }
    }

    private Vector3 StartHeadBob()
    {
        float t = Mathf.InverseLerp(1f, 99f, Player.statHandler.Stat.SPD);  // STP가 1일 때 0, 99일 때 1
        float inverseEffect = 1f - t;  // 반비례 효과

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * inverseEffect, Smooth * Time.deltaTime);
        transform.localPosition += pos;
        return pos;
    }
}
