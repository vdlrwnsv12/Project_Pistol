using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirialTarget : BaseTarget
{
    protected override void Start()
    {
        base.Start();

        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.cyan;
        }
    }
}
