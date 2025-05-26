using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Step References")]
    public GameObject scissors;
    public GameObject cable;

    [Header("Inventory UI")]
    public GameObject inventoryIconPrefab;   // The USB icon prefab
    public Transform inventorySlot;          // UI slot where icon lands

    [Header("Messages")]
    public TextMeshProUGUI pickupMessage;    // “You have a USB cable” message
    public float messageDuration = 2f;

    private bool scissorsMoved = false;
    private bool cableCut = false;

    // Called from ScissorsInteraction.cs
    public void EnableScissorPush()
    {
        if (scissors != null)
        {
            scissorsMoved = true;
            Debug.Log("Scissors movement started (flag set).");
        }
    }

    // Called from CutPromptTrigger.cs
    public void CutCable()
    {
        if (cableCut) return;

        cableCut = true;

        if (cable != null)
        {
            Destroy(cable);
            Debug.Log("Cable destroyed.");
        }

        if (pickupMessage != null)
        {
            pickupMessage.text = "You have a USB cable";
            pickupMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay());
        }

        if (inventoryIconPrefab != null && inventorySlot != null)
        {
            StartCoroutine(MoveIconToInventory());
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        pickupMessage.gameObject.SetActive(false);
    }

    private IEnumerator MoveIconToInventory()
    {
        GameObject icon = Instantiate(inventoryIconPrefab, inventorySlot.parent);
        RectTransform iconRect = icon.GetComponent<RectTransform>();

        // Start from random-ish spawn point (optional)
        iconRect.position = inventorySlot.position + new Vector3(0, 200, 0);
        Vector3 start = iconRect.position;
        Vector3 end = inventorySlot.position;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            iconRect.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        iconRect.SetParent(inventorySlot);
        iconRect.anchoredPosition = Vector2.zero;
    }

    // Optional public getter
    public bool HasScissorsMoved()
    {
        return scissorsMoved;
    }
}
