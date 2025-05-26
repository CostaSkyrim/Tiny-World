using UnityEngine;

public class ScissorsTriggerProxy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<ScissorsInteraction>().PlayerEnteredTopZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<ScissorsInteraction>().PlayerExitedTopZone();
        }
    }
}
