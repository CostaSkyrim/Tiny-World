using UnityEngine;

public class CutPromptTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject cutPromptUI;
    public GameObject player;
    public GameObject scissorsObject;
    public MeshRenderer[] scissorsOutlines;
    public MeshRenderer cableOutline;
    public InventoryUI inventoryUI;

    [Header("Distance Thresholds")]
    public float maxPlayerX = 7f;
    public float maxPlayerZ = 5f;
    public float maxScissors = 5f; // full 3D for scissors

    private bool promptShown = false;
    private bool cutPerformed = false;

    private void Start()
    {
        if (cutPromptUI != null)
        {
            cutPromptUI.SetActive(false);
            Debug.Log("[CutPromptTrigger] UI prompt hidden at start");
        }

        SetOutlines(false);
    }

    private void Update()
    {
        if (cutPerformed) return;

        // Check player proximity (X and Z separately)
        Vector3 playerPos = player.transform.position;
        Vector3 cablePos = transform.position;

        float dx = Mathf.Abs(playerPos.x - cablePos.x);
        float dz = Mathf.Abs(playerPos.z - cablePos.z);
        bool playerInRange = dx <= maxPlayerX && dz <= maxPlayerZ;

        Debug.Log($"[CutPromptTrigger] Player dx: {dx:F2} (max {maxPlayerX}), dz: {dz:F2} (max {maxPlayerZ})");

        // Check scissors proximity (regular 3D distance)
        float distToScissors = Vector3.Distance(scissorsObject.transform.position, transform.position);
        bool scissorsInRange = distToScissors <= maxScissors;

        Debug.Log($"[CutPromptTrigger] Scissors distance: {distToScissors:F2} / max: {maxScissors}");

        // Show prompt if both in range
        if (playerInRange && scissorsInRange)
        {
            if (!promptShown)
            {
                cutPromptUI?.SetActive(true);
                SetOutlines(true);
                promptShown = true;
                Debug.Log("[CutPromptTrigger] Player and scissors in range. Showing UI and outlines.");
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
                cutPromptUI?.SetActive(false);
                SetOutlines(false);
                promptShown = false;
                Debug.Log("[CutPromptTrigger] One or both out of range. Hiding UI and outlines.");
            }
        }
    }

    void SetOutlines(bool state)
    {
        foreach (var outline in scissorsOutlines)
        {
            if (outline != null)
                outline.enabled = state;
        }

        if (cableOutline != null)
            cableOutline.enabled = state;
    }

    void PerformCut()
    {
        cutPerformed = true;
        cutPromptUI?.SetActive(false);
        SetOutlines(false);

        if (inventoryUI != null)
        {
            inventoryUI.ShowCablePickup();
            Debug.Log("[CutPromptTrigger] Inventory UI triggered.");
        }
        else
        {
            Debug.LogWarning("[CutPromptTrigger] Inventory UI reference is missing!");
        }

        Debug.Log("[CutPromptTrigger] Destroying cable.");
        Destroy(gameObject);
    }
}
