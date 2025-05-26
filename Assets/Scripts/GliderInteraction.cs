using StarterAssets;
using UnityEngine;
using System.Collections;

public class GliderInteraction : MonoBehaviour
{
    public Canvas promptCanvas;
    public MeshRenderer[] outlineRenderers;

    private ThirdPersonController player;
    public Transform gliderTargetPosition;
    public GameManager gameManager;
    public DeathFadeUI deathFadeUI;
    private CharacterController charController;

    private bool playerInRange = false;
    // private bool hasUSB = false;

    void Start()
    {
        player = FindObjectOfType<ThirdPersonController>();
        if (player == null)
            Debug.LogError("GliderInteraction: No ThirdPersonController found in scene!");
        else
            charController = player.GetComponent<CharacterController>();
        
        if (promptCanvas != null)
            promptCanvas.gameObject.SetActive(false);

        SetOutlines(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // hasMoved = true;

            promptCanvas.gameObject.SetActive(false);
            SetOutlines(false);

            if (gameManager != null)
            {
                gameManager.gliderEnabled = true;
            }

            StartCoroutine(StartGlider());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            promptCanvas.gameObject.SetActive(true);
            SetOutlines(true);
            Debug.Log("Player entered glider range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptCanvas.gameObject.SetActive(false);
            SetOutlines(false);
            Debug.Log("Player left glider range.");
        }
    }

    private System.Collections.IEnumerator StartGlider()
    {
        if (deathFadeUI != null)
        {
            yield return StartCoroutine(deathFadeUI.FadeIn());
        }

        // CharacterController cc = player.GetComponent<CharacterController>();
        // if (cc != null) cc.enabled = false;
            charController.enabled = false;
    
        if (player != null && gliderTargetPosition != null)
        {
            Debug.Log($"[Glider] Teleporting player from {player.transform.position} to {gliderTargetPosition.position}");
            player.transform.position = gliderTargetPosition.position;
            player.transform.rotation = gliderTargetPosition.rotation;
            Debug.Log($"[Glider] Player now at {player.transform.position}");
        }
        
        charController.enabled = true;

        // if (cc != null) cc.enabled = true;

        if (deathFadeUI != null)
        {
            yield return StartCoroutine(deathFadeUI.FadeOut());
        }

        // player.enabled = true;

        Debug.Log("Glider started...");
    }

    private void SetOutlines(bool state)
    {
        foreach (var renderer in outlineRenderers)
        {
            if (renderer != null)
                renderer.enabled = state;
        }
    }
}
