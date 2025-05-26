using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI pickupMessageText;
    public Image usbIcon;
    public Transform targetPosition;

    private void Start()
    {
        pickupMessageText.gameObject.SetActive(false);
        usbIcon.gameObject.SetActive(false);
    }

    public void ShowCablePickup()
    {
        StartCoroutine(CablePickupSequence());
    }

    private IEnumerator CablePickupSequence()
    {
        pickupMessageText.text = "You have a USB cable";
        pickupMessageText.gameObject.SetActive(true);
        usbIcon.gameObject.SetActive(true);

        // Animate the USB icon to its final position
        Vector3 start = usbIcon.rectTransform.position;
        Vector3 end = targetPosition.position;

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            usbIcon.rectTransform.position = Vector3.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        usbIcon.rectTransform.position = end;

        yield return new WaitForSeconds(2f);
        pickupMessageText.gameObject.SetActive(false);
    }
}
