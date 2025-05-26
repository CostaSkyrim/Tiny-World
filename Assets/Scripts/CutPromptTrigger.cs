using UnityEngine;

public class CutPromptTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject cutPromptUI;          // "E to Cut" UI prompt
    public GameObject player;               // Assign Player GameObject
    public GameObject scissorsObject;       // Assign Scissors parent GameObject
    public MeshRenderer[] scissorsOutlines; // Optional: Scissors glow
    public MeshRenderer cableOutline;       // Optional: Cable glow

    [Header("Settings")]
    public float playerDistance = 2f;       // Distance threshold for player
    public float scissorsDistance = 1.5f;   // Distance threshold for scissors

    private bool promptShown = false;
    private bool cutPerformed = false;

    private void Start()
    {
        Debug.Log("[CutPromptTrigger] Initialized");

        if (cutPromptUI != null)
        {
            cutPromptUI.SetActive(false);
            Debug.Log("[CutPromptTrigger] UI prompt hidden at start");
        }
        else
        {
            Debug.LogWarning("[CutPromptTrigger] cutPromptUI is not assigned!");
        }

        SetOutlines(false);
    }

    private void Update()
    {
        if (cutPerformed)
        {
            Debug.Log("[CutPromptTrigger] Cut already performed. Skipping update.");
            return;
        }

        float distToPlayer = Vector3.Distance(player.transform.position, transform.position);
        float distToScissors = Vector3.Distance(scissorsObject.transform.position, transform.position);

        Debug.Log($"[CutPromptTrigger] Player distance: {distToPlayer:F2} / Required: {playerDistance}");
        Debug.Log($"[CutPromptTrigger] Scissors distance: {distToScissors:F2} / Required: {scissorsDistance}");

        if (distToPlayer <= playerDistance && distToScissors <= scissorsDistance)
        {
            if (!promptShown)
            {
                Debug.Log("[CutPromptTrigger] Both player and scissors are close. Showing cut prompt and outlines.");
                cutPromptUI.SetActive(true);
                SetOutlines(true);
                promptShown = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("[CutPromptTrigger] Player pressed E. Performing cut.");
                PerformCut();
            }
        }
        else
        {
            if (promptShown)
            {
                Debug.Log("[CutPromptTrigger] One or both objects are too far. Hiding prompt and outlines.");
                cutPromptUI.SetActive(false);
                SetOutlines(false);
                promptShown = false;
            }
        }
    }

    void SetOutlines(bool state)
    {
        Debug.Log($"[CutPromptTrigger] Setting outline state to: {state}");

        foreach (var outline in scissorsOutlines)
        {
            if (outline != null)
            {
                outline.enabled = state;
                Debug.Log($"[CutPromptTrigger] Outline on '{outline.name}' set to {state}");
            }
            else
            {
                Debug.LogWarning("[CutPromptTrigger] Null outline found in scissorsOutlines array");
            }
        }

        if (cableOutline != null)
        {
            cableOutline.enabled = state;
            Debug.Log($"[CutPromptTrigger] Cable outline set to {state}");
        }
        else
        {
            Debug.LogWarning("[CutPromptTrigger] cableOutline is not assigned");
        }
    }

    void PerformCut()
    {
        cutPerformed = true;
        cutPromptUI.SetActive(false);
        SetOutlines(false);

        Debug.Log("[CutPromptTrigger] Cut performed! Triggering inventory update...");

        InventoryUI inventory = FindObjectOfType<InventoryUI>();
        if (inventory != null)
        {
            inventory.ShowCablePickup();
            Debug.Log("[CutPromptTrigger] InventoryUI found and triggered.");
        }
        else
        {
            Debug.LogError("[CutPromptTrigger] InventoryUI not found in scene!");
        }

        Debug.Log("[CutPromptTrigger] Destroying cable GameObject.");
        Destroy(gameObject);
    }
}
