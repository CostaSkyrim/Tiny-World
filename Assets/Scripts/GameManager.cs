using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Death Settings")]
    public bool freezeTimeOnDeath = true;
    public float deathDelay = 2f;

    private bool isDead = false;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Optional: Don't destroy on scene load
        // DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Call this from any hazard or system to kill the player.
    /// </summary>
    public void KillPlayer(string reason)
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($" Player died: {reason}");

        // Disable input (optional: use your input system)
        StarterAssetsInputs input = FindObjectOfType<StarterAssetsInputs>();
        if (input != null) input.enabled = false;

        // Optional: play death animation
        ThirdPersonController player = FindObjectOfType<ThirdPersonController>();
        if (player != null)
        {
            Animator anim = player.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Die"); // Make sure you have a "Die" trigger in Animator
            }
        }

        // Optional: freeze time or delay restart
        if (freezeTimeOnDeath)
            Time.timeScale = 0f;
        else
            Invoke(nameof(RestartLevel), deathDelay);
    }

    /// <summary>
    /// Restarts the current scene (if time isn't frozen).
    /// </summary>
    private void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Public method to be called from a UI button to restart.
    /// </summary>
    public void RestartManually()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
