using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Cable & Scissors")]
    public GameObject cable;

    [Header("Inventory")]
    public GameObject usbIconPrefab; // UI Image prefab
    public Transform inventorySlot;  // The Image or Empty inside bottom right slot

    [Header("UI Message")]
    public TextMeshProUGUI pickupMessageText;

    public void CutCable()
    {
        Destroy(cable);

        // Show message
        pickupMessageText.text = "You have a USB cable";
        pickupMessageText.gameObject.SetActive(true);
        StartCoroutine(FadeOutText(pickupMessageText, 2f));

        // Spawn icon and animate it to inventory
        GameObject icon = Instantiate(usbIconPrefab, inventorySlot.parent); // place in same canvas
        icon.transform.position = Input.mousePosition; // OR: scissors world → screenPoint → canvas space
        StartCoroutine(MoveToInventory(icon));
    }

    IEnumerator MoveToInventory(GameObject icon)
    {
        Vector3 start = icon.transform.position;
        Vector3 end = inventorySlot.position;

        float duration = 0.8f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            icon.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        icon.transform.position = end;
        icon.transform.SetParent(inventorySlot); // parent it to slot
        icon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    IEnumerator FadeOutText(TextMeshProUGUI text, float delay)
    {
        yield return new WaitForSeconds(delay);

        float duration = 1f;
        float elapsed = 0f;
        Color c = text.color;

        while (elapsed < duration)
        {
            text.color = new Color(c.r, c.g, c.b, Mathf.Lerp(1, 0, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        text.gameObject.SetActive(false);
        text.color = c; // Reset for next time
    }
}
