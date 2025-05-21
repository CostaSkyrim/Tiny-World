using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioClip deskMusic;
    public AudioClip floorMusic;

    private AudioClip currentMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep AudioManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource.loop = true; // Make sure the background music loops
    }

    // Switch to desk music
    public void SwitchToDeskMusic()
    {
        if (currentMusic != deskMusic)
        {
            currentMusic = deskMusic;
            musicSource.clip = deskMusic;
            musicSource.Play();
        }
    }

    // Switch to floor music
    public void SwitchToFloorMusic()
    {
        if (currentMusic != floorMusic)
        {
            currentMusic = floorMusic;
            musicSource.clip = floorMusic;
            musicSource.Play();
        }
    }
}
