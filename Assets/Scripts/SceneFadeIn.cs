using UnityEngine;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.5f;
    public bool fadeOnStart = true;

    private void Start()
    {
        if (fadeOnStart)
        {
            StartCoroutine(FadeFromBlack());
        }
    }

    public IEnumerator FadeFromBlack()
    {
        fadeCanvasGroup.alpha = 1f;

        float t = 0f;
        while (t < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
    }
}
