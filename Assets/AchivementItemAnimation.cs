using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AchivementItemAnimation : MonoBehaviour
{
    private RectTransform rt;
    public Image image;

    void OnEnable()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        if (rt == null) Debug.LogWarning("RectTransform null");
        if (image == null) Debug.LogWarning("Image null");

        if (rt != null && image != null)
            StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Vector3 basePos = rt.localPosition;
        Vector3 startPos = basePos + new Vector3(0f, -100f, 0f);
        Vector3 midPos = basePos;
        Vector3 endPos = basePos + new Vector3(400f, 0f, 0f);

        float riseDuration = 0.5f;
        float waitDuration = 1f;
        float exitDuration = 0.3f;

        rt.localPosition = startPos;

        // 페이드인 시작
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        float elapsed = 0f;
        while (elapsed < riseDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / riseDuration;
            rt.localPosition = Vector3.Lerp(startPos, midPos, t);
            color.a = t;
            image.color = color;
            yield return null;
        }

        rt.localPosition = midPos;
        color.a = 1f;
        image.color = color;

        yield return new WaitForSeconds(waitDuration);

        // 오른쪽으로 나가면서 페이드아웃
        elapsed = 0f;
        while (elapsed < exitDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / exitDuration;
            rt.localPosition = Vector3.Lerp(midPos, endPos, t);
            color.a = 1f - t;
            image.color = color;
            yield return null;
        }

        rt.localPosition = endPos;
        color.a = 0f;
        image.color = color;

        // 필요시 여기서 비활성화 or 풀 반환
        // gameObject.SetActive(false);
    }
}
