using UnityEngine;

public class CableBlockerTrigger : MonoBehaviour
{
    public GameObject messagePanel; // UI panel in world space or screen space
    public string message = "I can't even jump over a cable. I need to cut it. Where are the scissors?";

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            messagePanel.SetActive(true);
            messagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            messagePanel.SetActive(false);
        }
    }
}
