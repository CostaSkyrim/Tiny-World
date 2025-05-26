using UnityEngine;
using TMPro;
using System.Collections;

public class CableBlockerTrigger : MonoBehaviour
{
    public TextMeshProUGUI tutorialMessageText;
    public CanvasGroup messageGroup;
    [TextArea]
    public string message = "I can't even jump over a cable. I need to cut it. Where are the scissors?";
    public float displayTime = 4f;
    public float fadeDuration = 1f;

    private bool hasShownMessage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasShownMessage) return;

        if (other.CompareTag("Player"))
        {
            hasShownMessage = true;
            StartCoroutine(ShowMessage());
        }
    }

    private IEnumerator ShowMessage()
    {
        tutorialMessageText.text = message;

        // Fade in
        yield return StartCoroutine(FadeCanvasGroup(messageGroup, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(displayTime);

        // Fade out
        yield return StartCoroutine(FadeCanvasGroup(messageGroup, 1f, 0f, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;
        canvasGroup.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (endAlpha == 0f)
            canvasGroup.gameObject.SetActive(false);
    }
}
