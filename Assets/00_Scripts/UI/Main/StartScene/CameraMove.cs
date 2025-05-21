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

    [Header("페이드 설정")]
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private float fadeDuration = 1f;

    [Tooltip("페이드 아웃이 시작되는 이동 지점 비율 (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    [SerializeField] private float fadeOutTriggerPercent = 0.9f;

    [Tooltip("페이드 인이 시작되는 이동 지점 비율 (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    [SerializeField] private float fadeInTriggerPercent = 0.1f;

    private void Start()
    {
        // if (targetCamera == null || fadeOverlay == null || moveStartPoints.Count != moveEndPoints.Count)
        // {
        //     Debug.LogError("CameraMove: 참조 누락 또는 리스트 수 불일치");
        //     enabled = false;
        //     return;
        // }

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

        bool fadeInStarted = false;
        bool fadeOutStarted = false;

        float fadeOutTriggerTime = moveDuration * fadeOutTriggerPercent;
        float fadeInTriggerTime = moveDuration * fadeInTriggerPercent;

        Coroutine fadeInRoutine = null;
        Coroutine fadeOutRoutine = null;

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
