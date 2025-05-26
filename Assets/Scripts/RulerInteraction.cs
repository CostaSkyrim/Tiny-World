using UnityEngine;
using TMPro;

public class RulerInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject interactionPromptUI;
    public MeshRenderer rulerRenderer;
    public Material defaultMaterial;
    public Material outlineMaterial;
    public Transform rulerTransform;       // Empty parent pivot of the ruler

    [Header("Rotation Settings")]
    public float targetYRotation = 90f;    // Angle to rotate toward on Y axis
    public float rotationSpeed = 60f;

    private bool playerInZone = false;
    private bool hasBeenPushed = false;
    private bool isRotating = false;

    private void Start()
    {
        if (interactionPromptUI != null)
            interactionPromptUI.SetActive(false);

        SetRulerMaterial(defaultMaterial);
    }

    private void Update()
    {
        if (playerInZone && !hasBeenPushed && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed – pushing the ruler.");
            hasBeenPushed = true;
            isRotating = true;
            interactionPromptUI.SetActive(false);
            SetRulerMaterial(defaultMaterial);
        }

        if (isRotating)
        {
            RotateRuler();
        }
    }

    private void RotateRuler()
    {
        float currentY = rulerTransform.localEulerAngles.y;
        float newY = Mathf.MoveTowardsAngle(currentY, targetYRotation, rotationSpeed * Time.deltaTime);

        rulerTransform.localEulerAngles = new Vector3(
            rulerTransform.localEulerAngles.x,
            newY,
            rulerTransform.localEulerAngles.z
        );

        if (Mathf.Abs(Mathf.DeltaAngle(currentY, targetYRotation)) < 0.1f)
        {
            isRotating = false;
            Debug.Log("Ruler finished rotating.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenPushed)
        {
            playerInZone = true;
            interactionPromptUI.SetActive(true);
            SetRulerMaterial(outlineMaterial);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            interactionPromptUI.SetActive(false);
            SetRulerMaterial(defaultMaterial);
        }
    }

    private void SetRulerMaterial(Material mat)
    {
        if (rulerRenderer != null && mat != null)
            rulerRenderer.material = mat;
    }
}
