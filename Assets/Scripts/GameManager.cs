using StarterAssets;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn Settings")]
    public Transform spawnPoint; // Assign in Inspector

    [Header("UI Fade")]
    public DeathFadeUI fadeUI; // Assign this in Inspector

    private ThirdPersonController player;
    private Animator anim;
    private StarterAssetsInputs input;
    private bool isDead = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        player = FindObjectOfType<ThirdPersonController>();
        anim = player.GetComponent<Animator>();
        input = player.GetComponent<StarterAssetsInputs>();
    }

    public void KillPlayer(string reason)
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($" Player died: {reason}");

        player = FindObjectOfType<ThirdPersonController>();
        anim = player.GetComponent<Animator>();
        input = player.GetComponent<StarterAssetsInputs>();

        input.move = Vector2.zero;
        input.enabled = false;

        Debug.Log("Setting animation trigger: " + reason);

        switch (reason)
        {
            case "Stepped in soda":
                anim.SetTrigger("deathLiquid");
                break;
            case "Caught by Roomba":
                anim.SetTrigger("deathRoomba");
                break;
            case "Fell from height":
                anim.SetTrigger("deathFall");
                break;
            default:
                anim.SetTrigger("deathFall");
                break;
        }

        Debug.Log("Trigger set");

        StartCoroutine(RespawnAfterDeathAnimation());
    }

    private IEnumerator RespawnAfterDeathAnimation()
    {
        yield return null;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        while (!state.IsName("Death_Liquid") && !state.IsName("Death_Fall") && !state.IsName("Death_Roomba"))
        {
            yield return null;
            state = anim.GetCurrentAnimatorStateInfo(0);
        }

        float animTime = state.length;
        Debug.Log($" Waiting for death animation: {animTime} seconds");
        yield return new WaitForSeconds(animTime);

        //  Fade to black before respawning
        if (fadeUI != null)
        {
            yield return StartCoroutine(fadeUI.FadeIn());
        }

        Debug.Log(" Respawning player...");

        // Move the player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        if (cc != null) cc.enabled = true;

        // Reset animation
        anim.Rebind();
        anim.Update(0f);

        // Short delay after repositioning
        yield return new WaitForSeconds(0.2f);

        //  Fade out from black
        if (fadeUI != null)
        {
            yield return StartCoroutine(fadeUI.FadeOut());
        }

        // Restore control
        input.enabled = true;
        isDead = false;

        Debug.Log(" Player respawned.");
    }
}
