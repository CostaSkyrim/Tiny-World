using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement;

public class ComicSlideManager : MonoBehaviour
{

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    [Header("Scene Transition")]
    public string nextSceneName;           // Set this in the Inspector
    public float delayAfterLastPanel = 2f; // Time to wait before switching

    [Header("Typewriter Effects")]
    public TypewriterEffect[] panelTextEffects;

    [Header("Panel Slide Settings")]
    public RectTransform panelContainer;      // Parent of all panels
    public int panelWidth = 1920;             // Width of each panel in pixels
    public float transitionDuration = 0.6f;   // How long each slide takes
    public float timeBetweenSlides = 4f;      // Seconds before advancing to next panel

    [Header("Video Players")]
    public VideoPlayer[] panelVideoPlayers;   // One VideoPlayer per panel

    private int currentIndex = 0;
    private bool isSliding = false;

    void Start()
    {
        PlayOnlyCurrentVideo();
        StartCoroutine(AutoAdvanceCutscene());
    }

    IEnumerator AutoAdvanceCutscene()
    {
        while (currentIndex < panelVideoPlayers.Length - 1)
        {
            yield return new WaitForSeconds(timeBetweenSlides);
            currentIndex++;
            StartCoroutine(SlideToPosition(-currentIndex * panelWidth));
            PlayOnlyCurrentVideo();
        }

        // Wait before fade
        yield return new WaitForSeconds(timeBetweenSlides);

        // Start fade-out
        yield return StartCoroutine(FadeToBlack());

        // Wait a moment, then load next scene
        yield return new WaitForSeconds(delayAfterLastPanel);
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeToBlack()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1;
    }

    IEnumerator SlideToPosition(float targetX)
    {
        isSliding = true;
        Vector2 startPos = panelContainer.anchoredPosition;
        Vector2 targetPos = new Vector2(targetX, startPos.y);
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            panelContainer.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / transitionDuration);
            yield return null;
        }

        panelContainer.anchoredPosition = targetPos;
        isSliding = false;
    }

    void PlayOnlyCurrentVideo()
    {
        for (int i = 0; i < panelVideoPlayers.Length; i++)
        {
            if (panelVideoPlayers[i] != null)
            {
                if (i == currentIndex)
                {
                    panelVideoPlayers[i].Stop();
                    panelVideoPlayers[i].Play();
                }
                else
                {
                    panelVideoPlayers[i].Stop();
                }
            }

            if (panelTextEffects.Length > i && panelTextEffects[i] != null)
            {
                if (i == currentIndex)
                    panelTextEffects[i].StartTyping();
                else
                    panelTextEffects[i].StopAllCoroutines(); // Kill other text animations
            }
        }
    }
}
