using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LiquidHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[LiquidHazard] Player detected — calling GameManager KillPlayer()");
            GameManager.Instance.KillPlayer("Stepped in soda");
        }
    }
}
