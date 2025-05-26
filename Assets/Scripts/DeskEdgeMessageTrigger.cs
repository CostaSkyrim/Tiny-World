using UnityEngine;
using TMPro;

public class DeskEdgeMessageTrigger : MonoBehaviour
{
    public CanvasGroup messageCanvasGroup; // Assign in Inspector
    public float fadeDuration = 1f;
    public float visibleDuration = 3f;

    private bool hasShown = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[DeskEdgeTrigger] OnTriggerEnter called by: " + other.name);

        if (!hasShown && other.CompareTag("Player"))
        {
            Debug.Log("[DeskEdgeTrigger] Player entered trigger zone.");
            hasShown = true;

            if (messageCanvasGroup == null)
            {
                Debug.LogError("[DeskEdgeTrigger] CanvasGroup is NOT assigned!");
                return;
            }

            StartCoroutine(FadeMessage());
        }
    }

    private System.Collections.IEnumerator FadeMessage()
    {
        Debug.Log("[DeskEdgeTrigger] Starting Fade In...");

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            messageCanvasGroup.alpha = alpha;
            Debug.Log($"[Fade In] Alpha: {alpha}");
            yield return null;
        }

        messageCanvasGroup.alpha = 1f;
        Debug.Log("[DeskEdgeTrigger] Message fully visible. Waiting...");

        yield return new WaitForSeconds(visibleDuration);

        Debug.Log("[DeskEdgeTrigger] Starting Fade Out...");

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            messageCanvasGroup.alpha = alpha;
            Debug.Log($"[Fade Out] Alpha: {alpha}");
            yield return null;
        }

        messageCanvasGroup.alpha = 0f;
        Debug.Log("[DeskEdgeTrigger] Message faded out.");
    }
}
