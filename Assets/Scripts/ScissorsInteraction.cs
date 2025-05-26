using UnityEngine;

public class ScissorsInteraction : MonoBehaviour
{
    public Canvas promptCanvas;
    public MeshRenderer[] outlineRenderers;      // Assign both outline renderers
    public Transform targetPosition;             // Where scissors should move
    public float moveSpeed = 1f;

    private bool playerInRange = false;
    private bool hasMoved = false;

    void Start()
    {
        if (promptCanvas != null)
            promptCanvas.gameObject.SetActive(false);

        SetOutlines(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasMoved)
        {
            hasMoved = true;

            promptCanvas.gameObject.SetActive(false);
            SetOutlines(false);

            StartCoroutine(MoveScissorsToTarget());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasMoved)
        {
            playerInRange = true;
            promptCanvas.gameObject.SetActive(true);
            SetOutlines(true);
            Debug.Log("Player entered scissors range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptCanvas.gameObject.SetActive(false);
            SetOutlines(false);
            Debug.Log("Player left scissors range.");
        }
    }

    private System.Collections.IEnumerator MoveScissorsToTarget()
    {
        Debug.Log("Scissors start moving...");
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Scissors reached the target.");
    }

    private void SetOutlines(bool state)
    {
        foreach (var renderer in outlineRenderers)
        {
            if (renderer != null)
                renderer.enabled = state;
        }
    }

    public void PlayerEnteredTopZone()
    {
        if (hasMoved) return;

        playerInRange = true;
        if (promptCanvas != null) promptCanvas.gameObject.SetActive(true);
        SetOutlines(true);
        Debug.Log("Player entered scissors top zone (via proxy).");
    }

    public void PlayerExitedTopZone()
    {
        playerInRange = false;
        if (promptCanvas != null) promptCanvas.gameObject.SetActive(false);
        SetOutlines(false);
        Debug.Log("Player exited scissors top zone (via proxy).");
    }
}
