using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DataDeclaration;// Scene enum 정의 위치
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// 흐린 배경 위에 좌측부터 점점 뚜렷해지는 로딩 로고 연출 → 완료 시 다음 씬 전환
/// </summary>
public class LoadingLogoFiller : MonoBehaviour
{
    [Header("로고 이미지")]
    [SerializeField] private Image logoImage; // 반드시 'Filled', 'Horizontal'

    // [Header("애니메이션 설정")]
    // [Tooltip("로딩 한 사이클 시간")]
    // [SerializeField] private float fillDuration = 2f;

    // [Tooltip("중간 정지 여부 (Fill 0.5에서 잠깐 멈춤)")]
    // [SerializeField] private bool stopAtMiddle = false;

    // [Tooltip("중간 정지 시간")]
    // [SerializeField] private float middleStopDuration = 1f;

    [Header("색상 알파 설정")]
    [SerializeField] private float startAlpha = 0.2f;
    [SerializeField] private float endAlpha = 1f;

    [Header("다음 씬")]
    [SerializeField] private DataDeclaration.Scene nextScene = DataDeclaration.Scene.Lobby;


    private void Awake()
    {
        nextScene = SceneLoadManager.NextScene;
        Debug.Log($"[LoadingLogoFiller] Awake → nextScene = {nextScene}");
    }
    private void Start()
    {
        if (logoImage == null)
        {
            Debug.LogError("logoImage가 비어 있습니다.");
            enabled = false;
            return;
        }

        if (logoImage.type != Image.Type.Filled)
        {
            Debug.LogWarning("logoImage는 Filled 타입이어야 합니다.");
        }

        logoImage.fillMethod = Image.FillMethod.Horizontal;
        logoImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        logoImage.fillAmount = 0f;

        // StartCoroutine(AnimateAndLoadScene());
        StartCoroutine(FillByLoadingLogo());
    }

    private IEnumerator FillByLoadingLogo()
    {
        float currentFill = 0f;
        float targetFill = 0f;
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync((int)nextScene, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        Debug.Log($"[LoadingLogoFiller] 씬 비동기 로딩 시작: {nextScene}");

        while (op.progress < 0.9f)
        {
            // 타겟 fill을 로딩 프로그레스 기준으로 설정 (0~0.9 → 0~1)
            targetFill = Mathf.Clamp01(op.progress / 0.9f);

            // 현재 fillAmount를 타겟 값으로 부드럽게 보간
            currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * 5f);
            logoImage.fillAmount = currentFill;

            SetAlpha(Mathf.Lerp(startAlpha, endAlpha, currentFill));

            yield return null;
        }
        while (currentFill < 0.995f)
        {
            currentFill = Mathf.Lerp(currentFill, 1f, Time.deltaTime * 5f);
            logoImage.fillAmount = currentFill;
            SetAlpha(Mathf.Lerp(startAlpha, endAlpha, currentFill));
            yield return null;
        }

        logoImage.fillAmount = 1f;
        SetAlpha(endAlpha);

        Debug.Log("[LoadingLogoFiller] 씬 로딩 거의 완료 (progress >= 0.9)");

        yield return new WaitForSeconds(2f);
        op.allowSceneActivation = true;

        Debug.Log("[LoadingLogoFiller] 씬 전환 시작");
        SceneLoadManager.Instance.OnSceneEnterComplete();
    }

    // private IEnumerator AnimateAndLoadScene()
    // {
    //     float elapsed = 0f;

    //     // 0 → 0.5
    //     while (elapsed < fillDuration / 2f)
    //     {
    //         elapsed += Time.deltaTime;
    //         float t = elapsed / (fillDuration / 2f);
    //         logoImage.fillAmount = Mathf.Lerp(0f, 0.5f, t);
    //         SetAlpha(Mathf.Lerp(startAlpha, (startAlpha + endAlpha) / 2f, t));
    //         yield return null;
    //     }

    //     logoImage.fillAmount = 0.5f;
    //     SetAlpha((startAlpha + endAlpha) / 2f);

    //     if (stopAtMiddle)
    //     {
    //         yield return new WaitForSeconds(middleStopDuration);
    //     }

    //     // 0.5 → 1.0
    //     elapsed = 0f;
    //     while (elapsed < fillDuration / 2f)
    //     {
    //         elapsed += Time.deltaTime;
    //         float t = elapsed / (fillDuration / 2f);
    //         logoImage.fillAmount = Mathf.Lerp(0.5f, 1f, t);
    //         SetAlpha(Mathf.Lerp((startAlpha + endAlpha) / 2f, endAlpha, t));
    //         yield return null;
    //     }

    //     logoImage.fillAmount = 1f;
    //     SetAlpha(endAlpha);

    //     Debug.Log("로딩 애니메이션 완료 → 다음 씬으로 전환");
    //     yield return new WaitForSeconds(0.5f); // 약간의 여유

    //     SceneLoadManager.Instance.LoadScene(nextScene);
    // }

    /// <summary>
    /// 이미지 색상의 알파만 설정
    /// </summary>
    private void SetAlpha(float a)
    {
        Color col = logoImage.color;
        col.a = a;
        logoImage.color = col;
    }
}
