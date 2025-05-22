using StarterAssets;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Transform spawnPoint; // Assign in Inspector

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

        // Refresh references in case they're lost
        player = FindObjectOfType<ThirdPersonController>();
        anim = player.GetComponent<Animator>();
        input = player.GetComponent<StarterAssetsInputs>();

        // Disable player input
        input.move = Vector2.zero;
        input.enabled = false;

        // Trigger appropriate death animation
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

        // Start respawn coroutine
        StartCoroutine(RespawnAfterDeathAnimation());
    }

    private System.Collections.IEnumerator RespawnAfterDeathAnimation()
    {
        yield return null;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        // Wait until we're in a death animation state
        while (!state.IsName("Death_Liquid") && !state.IsName("Death_Fall") && !state.IsName("Death_Roomba"))
        {
            yield return null;
            state = anim.GetCurrentAnimatorStateInfo(0);
        }

        Debug.Log($" Waiting for death animation: {state.length} seconds");

        yield return new WaitForSeconds(state.length);

        Debug.Log(" Respawning player...");

        // Respawn at spawn point
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        // Reset Animator
        anim.Rebind();
        anim.Update(0f);

        // Re-enable player input
        input.enabled = true;
        isDead = false;

        Debug.Log(" Player respawned.");
    }
}