using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 흐린 배경 위에 좌측부터 점점 뚜렷해지는 로딩 로고 연출
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
    [SerializeField] private float startAlpha = 0.2f;  // 흐림 시작
    [SerializeField] private float endAlpha = 1f;      // 뚜렷한 상태

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

        StartCoroutine(AnimateLogo());
    }

    private IEnumerator AnimateLogo()
    {
        while (true)
        {
            float elapsed = 0f;

            // 0 → 0.5
            while (elapsed < fillDuration / 2f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (fillDuration / 2f);
                float fill = Mathf.Lerp(0f, 0.5f, t);
                float alpha = Mathf.Lerp(startAlpha, (startAlpha + endAlpha) / 2f, t);

                logoImage.fillAmount = fill;
                SetAlpha(alpha);
                yield return null;
            }

            logoImage.fillAmount = 0.5f;
            SetAlpha((startAlpha + endAlpha) / 2f);

            if (stopAtMiddle)
            {
                yield return new WaitForSeconds(middleStopDuration);
            }

            // 0.5 → 1.0
            elapsed = 0f;
            while (elapsed < fillDuration / 2f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (fillDuration / 2f);
                float fill = Mathf.Lerp(0.5f, 1f, t);
                float alpha = Mathf.Lerp((startAlpha + endAlpha) / 2f, endAlpha, t);

                logoImage.fillAmount = fill;
                SetAlpha(alpha);
                yield return null;
            }

            logoImage.fillAmount = 1f;
            SetAlpha(endAlpha);

            // 반복을 위한 초기화
            yield return new WaitForSeconds(0.5f);
            logoImage.fillAmount = 0f;
            SetAlpha(startAlpha);
        }
    }

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
