using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : MonoBehaviour
{
    public float detectRange = 5f;
    public float checkInterval = 0.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            CheckNearbyTargets();
        }
    }

    void CheckNearbyTargets()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRange);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Target"))
            {
                BaseTarget target = hit.GetComponent<BaseTarget>();
                if (target != null)
                {
                    target.OnPlayerEnteredRange();
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}