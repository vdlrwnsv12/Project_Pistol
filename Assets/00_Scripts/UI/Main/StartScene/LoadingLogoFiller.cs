using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DataDeclaration;
using System;
using UnityEngine.SceneManagement;
using System.Threading; // Scene enum 정의 위치

/// <summary>
/// 흐린 배경 위에 좌측부터 점점 뚜렷해지는 로딩 로고 연출 → 완료 시 다음 씬 전환
/// </summary>
public class LoadingLogoFiller : MonoBehaviour
{
    [Header("로고 이미지")]
    [SerializeField] private Image logoImage; // 반드시 'Filled', 'Horizontal'

    [Header("애니메이션 설정")]
    [Tooltip("로딩 한 사이클 시간")]
    [SerializeField] private float fillDuration = 2f;

    [Tooltip("중간 정지 여부 (Fill 0.5에서 잠깐 멈춤)")]
    [SerializeField] private bool stopAtMiddle = false;

    [Tooltip("중간 정지 시간")]
    [SerializeField] private float middleStopDuration = 1f;

    [Header("색상 알파 설정")]
    [SerializeField] private float startAlpha = 0.2f;
    [SerializeField] private float endAlpha = 1f;

    [Header("다음 씬")]
    [SerializeField] private DataDeclaration.Scene nextScene = DataDeclaration.Scene.Lobby;


    private void Awake()
    {
        if (!Enum.IsDefined(typeof(DataDeclaration.Scene), (int)nextScene))
        {
            nextScene = SceneLoadManager.NextScene;
        }
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
        float elapsed = 0f;

        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync((int)nextScene);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            if (progress >= 0.5f && progress - Time.deltaTime / fillDuration < 0.5f)//로고 반정도 채워지면 0.5초 대기
            {
                logoImage.fillAmount = 05f;
                SetAlpha(Mathf.Lerp(startAlpha, endAlpha, 0.5f));
                yield return new WaitForSeconds(0.5f);
            }
            
            logoImage.fillAmount = progress;
            SetAlpha(Mathf.Lerp(startAlpha, endAlpha, progress));

            elapsed += Time.deltaTime;
            yield return null;
        }

        while (elapsed < middleStopDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        logoImage.fillAmount = 1f;
        SetAlpha(endAlpha);

        yield return new WaitForSeconds(2f);

        op.allowSceneActivation = true;
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
