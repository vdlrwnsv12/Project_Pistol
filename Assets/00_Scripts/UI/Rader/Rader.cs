using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rader : MonoBehaviour
{
    public Transform raderCenter;
    public RectTransform raderPanel;
    public RectTransform sweepLine;
    public GameObject blipPrefab;
    public RectTransform blipContainer;

    public float sweepSpeed = 90f;
    public float raderRange = 100f;
    public float detectionAngleThreshold = 1.5f;
    public float blipLifetime = 1f;
    public LayerMask targetLayer;

    private float currentAngle = 0f;
    private float lastAngle = 0f;

    // 감지된 타겟 저장용
    private HashSet<Transform> detectedTargets = new HashSet<Transform>();

    void Update()
    {
        DetectTargets();

        // 레이더 UI 회전 (플레이어 회전 따라감)
        Vector3 euler = raderCenter.eulerAngles;
        raderPanel.localRotation = Quaternion.Euler(0, 0, euler.y);

        // 회전 막대 회전
        lastAngle = currentAngle;
        currentAngle += sweepSpeed * Time.deltaTime;
        currentAngle %= 360f;

        sweepLine.rotation = Quaternion.Euler(0, 0, -currentAngle);

        // 한 바퀴 회전 완료 시, 감지 기록 초기화
        if (lastAngle > currentAngle)  // 360도 넘어간 경우
        {
            detectedTargets.Clear();
        }
    }

    void DetectTargets()
    {
        Collider[] targets = Physics.OverlapSphere(raderCenter.position, raderRange, targetLayer);
        foreach (var target in targets)
        {
            if (detectedTargets.Contains(target.transform))
                continue;

            Vector3 dir = target.transform.position - raderCenter.position;
            dir.y = 0;
            if (dir.magnitude < 0.01f) continue;

            float baseAngle = raderCenter.eulerAngles.y;
            float worldAngleToTarget = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float relativeAngle = (worldAngleToTarget - baseAngle + 360f) % 360f;

            float deltaAngle = Mathf.DeltaAngle(currentAngle, relativeAngle);

            if (Mathf.Abs(deltaAngle) <= detectionAngleThreshold)
            {
                float radarRadius = raderPanel.rect.width / 2f;
                float distancePercent = Mathf.Clamp01(dir.magnitude / raderRange);

                float angleRad = currentAngle * Mathf.Deg2Rad;
                Vector2 blipPosition = new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad)) * radarRadius * distancePercent;

                GameObject blip = Instantiate(blipPrefab, blipContainer);
                blip.GetComponent<RectTransform>().anchoredPosition = blipPosition;

                Destroy(blip, blipLifetime);

                detectedTargets.Add(target.transform);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (raderCenter == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(raderCenter.position, raderRange);
    }
}
