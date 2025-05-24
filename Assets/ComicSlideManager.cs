using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class ComicSlideManager : MonoBehaviour
{
    [Header("Panel Slide Settings")]
    public RectTransform panelContainer; // The parent of all panels
    public int panelWidth = 1920;         // Fixed width of each panel
    public float transitionDuration = 0.6f;

    [Header("Video Players")]
    public VideoPlayer[] panelVideoPlayers; // One VideoPlayer per panel

    private int currentIndex = 0;
    private bool isSliding = false;

    void Start()
    {
        PlayOnlyCurrentVideo(); // Start video on Panel 0
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SlideNext();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SlidePrevious();
        }
    }

    public void SlideNext()
    {
        if (isSliding || currentIndex >= panelVideoPlayers.Length - 1)
            return;

        currentIndex++;
        StartCoroutine(SlideToPosition(-currentIndex * panelWidth));
        PlayOnlyCurrentVideo();
    }

    public void SlidePrevious()
    {
        if (isSliding || currentIndex <= 0)
            return;

        currentIndex--;
        StartCoroutine(SlideToPosition(-currentIndex * panelWidth));
        PlayOnlyCurrentVideo();
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
            if (panelVideoPlayers[i] == null) continue;

            if (i == currentIndex)
            {
                panelVideoPlayers[i].Stop(); // Restart from beginning
                panelVideoPlayers[i].Play();
            }
            else
            {
                panelVideoPlayers[i].Stop(); // Or use Pause() to resume later
            }
        }
    }
}
