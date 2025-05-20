using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ScoreTextAnimation : MonoBehaviour
{
    private RectTransform animatedRt;
    public Image image;

    void OnEnable()
    {
        Transform child = transform.childCount > 0 ? transform.GetChild(0) : null;

        if (child == null)
        {
            Debug.LogWarning("Child가 없음");
            return;
        }

        animatedRt = child.GetComponent<RectTransform>();
        image = child.GetComponent<Image>();

        if (animatedRt == null) Debug.LogWarning("animatedRt null");
        if (image == null) Debug.LogWarning("image null");

        if (animatedRt != null && image != null)
            StartCoroutine(AnimateScoreEntry(animatedRt, image));
    }

    private IEnumerator AnimateScoreEntry(RectTransform rt, Image image)
    {
        Vector3 startPos = new Vector3(0, -70f, 0);
        Vector3 midPos = Vector3.zero;
        float riseDuration = 0.5f;
        float elapsed = 0f;

        rt.localPosition = startPos;
        Color color = image.color;
        color.a = 0f;
        image.color = color;

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
        color.a = 1;
        image.color = color;

        yield return new WaitForSeconds(1f);

        Vector3 leftPos = new Vector3(-400f, 0f, 0f);
        float shiftDuration = 0.3f;
        elapsed = 0f;

        while (elapsed < shiftDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / shiftDuration;
            rt.localPosition = Vector3.Lerp(midPos, leftPos, t);
            color.a = 1f - t;
            image.color = color;

            yield return null;
        }
        rt.localPosition = leftPos;
    }
}
