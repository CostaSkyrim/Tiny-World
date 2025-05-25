using UnityEngine;

public class ScissorsInteraction : MonoBehaviour
{
    public Canvas promptCanvas;
    public GameObject[] outlineMeshes; // Assign both outline objects here

    private void Start()
    {
        if (promptCanvas != null)
            promptCanvas.gameObject.SetActive(false);

        foreach (var mesh in outlineMeshes)
        {
            if (mesh != null)
                mesh.SetActive(false);
        }

        Debug.Log("ScissorsInteraction initialized with " + outlineMeshes.Length + " outline meshes.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone.");

            if (promptCanvas != null)
                promptCanvas.gameObject.SetActive(true);

            foreach (var mesh in outlineMeshes)
            {
                if (mesh != null)
                    mesh.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger zone.");

            if (promptCanvas != null)
                promptCanvas.gameObject.SetActive(false);

            foreach (var mesh in outlineMeshes)
            {
                if (mesh != null)
                    mesh.SetActive(false);
            }
        }
    }
}
