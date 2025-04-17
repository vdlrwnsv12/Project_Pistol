using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeadBob : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0.001f, 0.01f)]
    public float Amount = 0.002f;

    [Range(1f, 30f)]
    public float Frequency = 10.0f;
    [Range(10f, 100f)]
    public float Smooth = 10.0f;
    void Start()
    {
        
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
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        transform.localPosition += pos;
        return pos;
    }
}
