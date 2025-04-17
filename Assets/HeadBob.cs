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
    private void Awake()
    {
        Player = GetComponentInParent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckForHeadbobTrigger();
    }

    void CheckForHeadbobTrigger()
    {
        float headMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        if(headMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        float t = Mathf.InverseLerp(1f, 99f, Player.statHandler.MoveSTP);  // STP가 1일 때 0, 99일 때 1
        float inverseEffect = 1f - t;  // 반비례 효과

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * inverseEffect, Smooth * Time.deltaTime);
        transform.localPosition += pos;
        return pos;
    }
}
