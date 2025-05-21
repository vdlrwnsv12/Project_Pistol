using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 카메라 연출에 페이드 인/아웃을 자연스럽게 삽입 (양 방향 조절 가능)
/// </summary>
public class CameraMove : MonoBehaviour
{
    [Header("카메라")]
    [SerializeField] private Camera targetCamera;

    [Header("이동 포인트")]
    [SerializeField] private List<Transform> moveStartPoints;
    [SerializeField] private List<Transform> moveEndPoints;

    [Header("연출 설정")]
    [SerializeField] private float moveDuration = 2f;

    private void Start()
    {
        StartCoroutine(PlaySequenceLoop());
    }

    private IEnumerator PlaySequenceLoop()
    {
        while (true)
        {
            for (int i = 0; i < moveStartPoints.Count; i++)
            {
                // 시작 지점 고정
                targetCamera.transform.position = moveStartPoints[i].position;
                targetCamera.transform.rotation = moveStartPoints[i].rotation;
                

                // 이동/회전 + 페이드 인 + 페이드 아웃 동시 수행
                yield return StartCoroutine(MoveWithFadeTiming(moveStartPoints[i], moveEndPoints[i]));
            }
        }
    }

    private IEnumerator MoveWithFadeTiming(Transform start, Transform end)
    {
        float elapsed = 0f;

        Vector3 startPos = start.position;
        Vector3 endPos = end.position;

        Quaternion startRot = start.rotation;
        Quaternion endRot = end.rotation;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);

            targetCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            targetCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        targetCamera.transform.position = endPos;
        targetCamera.transform.rotation = endRot;
    }
}
