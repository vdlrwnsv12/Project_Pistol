using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetectSensor : MonoBehaviour
{
    private Transform raderCenter;
    [SerializeField]private RectTransform blipContainer;
    [SerializeField]private RectTransform raderPanel;
    [SerializeField]private GameObject blipPrefab;

    [SerializeField]private float raderRange = 100f;
    [SerializeField]private LayerMask targetLayer;

    void Awake()
{
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
    {
        raderCenter = player.transform;
    }
    else
    {
        Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
    }
}

    void Update()
    {
        DetectTargets();

        // 레이더 UI 회전 (플레이어 회전 따라감)
        Vector3 euler = raderCenter.eulerAngles;
        raderPanel.localRotation = Quaternion.Euler(0, 0, euler.y);

    }

    void DetectTargets()
    {
        foreach (Transform child in blipContainer)
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
            float radarRadius = blipContainer.rect.width / 2f;

            Vector2 blipPos = new Vector2(offset.x, offset.z).normalized * radarRadius * distancePercent;

            GameObject blip = Instantiate(blipPrefab, blipContainer);
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

