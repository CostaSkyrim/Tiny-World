using UnityEngine;

public class CutPromptTrigger : MonoBehaviour
{
    public GameObject cutPrompt; // "E to Cut" UI
    public GameObject scissors, cable;
    public Material glowMaterial;
    private bool inZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == scissors)
        {
            cutPrompt.SetActive(true);
            scissors.GetComponent<Renderer>().material = glowMaterial;
            cable.GetComponent<Renderer>().material = glowMaterial;
            inZone = true;
        }
    }

    void Update()
    {
        if (inZone && Input.GetKeyDown(KeyCode.E))
        {
            cutPrompt.SetActive(false);
            FindObjectOfType<TutorialManager>().CutCable();
        }
    }
}
