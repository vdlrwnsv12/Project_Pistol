using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetectSensor : MonoBehaviour
{
    public Transform raderCenter;
    public RectTransform raderPanel;
    public GameObject blipPrefab;

    public float raderRange = 100f;
    public LayerMask targetLayer;


    // 감지된 타겟 저장용
    private HashSet<Transform> detectedTargets = new HashSet<Transform>();

    void Update()
    {
        DetectTargets();

        // 레이더 UI 회전 (플레이어 회전 따라감)
        Vector3 euler = raderCenter.eulerAngles;
        raderPanel.localRotation = Quaternion.Euler(0, 0, euler.y);

    }

    void DetectTargets()
    {
        foreach (Transform child in raderPanel)
        {
            if (child.CompareTag("Props"))
            {
                Destroy(child.gameObject);
            }
        }

        Collider[] targets = Physics.OverlapSphere(raderCenter.position, raderRange, targetLayer);
        foreach (var target in targets)
        {
            Vector3 offset = target.transform.position - raderCenter.position;
            offset.y = 0f;

            float distance = offset.magnitude;
            if (distance > raderRange) continue;

            float distancePercent = distance / raderRange;
            float radarRadius = raderPanel.rect.width / 2f;

            Vector2 blipPos = new Vector2(offset.x, offset.z).normalized * radarRadius * distancePercent;

            GameObject blip = Instantiate(blipPrefab, raderPanel);
            blip.GetComponent<RectTransform>().anchoredPosition = blipPos;
        }
    }

    void OnDrawGizmos()
    {
        if (raderCenter == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(raderCenter.position, raderRange);
    }
}
