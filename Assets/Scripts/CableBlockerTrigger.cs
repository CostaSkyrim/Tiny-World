using UnityEngine;

public class CableBlockerTrigger : MonoBehaviour
{
    public GameObject messagePanel; // Assign in Inspector
    public string message = "I can't even jump over a cable. I need to cut it. Where are the scissors?";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the cable trigger zone.");

            messagePanel.SetActive(true);
            messagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exited by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the cable trigger zone.");

            messagePanel.SetActive(false);
        }
    }
}
