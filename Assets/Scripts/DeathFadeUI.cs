using UnityEngine;
using TMPro; // if using TextMeshPro
using System.Collections;

public class DeathFadeUI : MonoBehaviour
{
    [Header("Fade Durations")]
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    [Header("Optional UI & Sound")]
    public TextMeshProUGUI fadeMessage; // Assign in Inspector
    public AudioSource fadeAudio;       // Assign in Inspector
    public string messageText = "WASTED! That was one energy drink too many..."; // Optional message

    private CanvasGroup canvasGroup;

    public AudioClip heartbeatClip; // Assign in Inspector

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        if (fadeMessage != null) fadeMessage.text = "";
    }

    public IEnumerator FadeIn()
    {
        Debug.Log("Fading in...");

        if (fadeMessage != null) fadeMessage.text = messageText;

        if (heartbeatClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(heartbeatClip);
            AudioManager.Instance.DuckMusic(0.2f, fadeInDuration); // Optional: lower music
        }

        float t = 0f;
        while (t < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            t += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeOut()
    {
        Debug.Log("Fading out...");
        if (fadeMessage != null) fadeMessage.text = "";

        float t = 0f;
        while (t < fadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
