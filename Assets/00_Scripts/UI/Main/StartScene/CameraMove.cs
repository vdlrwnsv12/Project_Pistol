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
        if (targetCamera == null || fadeOverlay == null || moveStartPoints.Count != moveEndPoints.Count)
        {
            Debug.LogError("CameraMove: 참조 누락 또는 리스트 수 불일치");
            enabled = false;
            return;
        }

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

                // 페이드 인 사전 처리: 강제 검정
                fadeOverlay.color = new Color(0f, 0f, 0f, 1f);

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

            // 페이드 인 트리거
            if (!fadeInStarted && elapsed >= fadeInTriggerTime)
            {
                fadeInRoutine = StartCoroutine(FadeIn());
                fadeInStarted = true;
            }

            // 페이드 아웃 트리거
            if (!fadeOutStarted && elapsed >= fadeOutTriggerTime)
            {
                fadeOutRoutine = StartCoroutine(FadeOut());
                fadeOutStarted = true;
            }

            yield return null;
        }

        targetCamera.transform.position = endPos;
        targetCamera.transform.rotation = endRot;

        if (fadeInRoutine != null) yield return fadeInRoutine;
        if (fadeOutRoutine != null) yield return fadeOutRoutine;
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = fadeOverlay.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            fadeOverlay.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        fadeOverlay.color = new Color(color.r, color.g, color.b, 0f);
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color color = fadeOverlay.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            fadeOverlay.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        fadeOverlay.color = new Color(color.r, color.g, color.b, 1f);
    }
}
