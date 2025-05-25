using UnityEngine;

public class ToggleOutline : MonoBehaviour
{
    private int defaultLayer;
    private int outlineLayer;

    void Start()
    {
        defaultLayer = LayerMask.NameToLayer("Default");
        outlineLayer = LayerMask.NameToLayer("Outline");
        SetOutline(false);
    }

    public void SetOutline(bool enable)
    {
        gameObject.layer = enable ? outlineLayer : defaultLayer;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SetOutline(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetOutline(false);
    }
}
