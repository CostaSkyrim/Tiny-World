using UnityEngine;
using System.Collections;

public class ClimbController : MonoBehaviour
{
    [Header("Climbing Settings")]
    public float climbSpeed = 2f;
    public float climbCheckDistance = 1.5f;
    public LayerMask climbableLayer;
    public float minClimbHeight = 0f;
    public float dismountDistance = 0.6f;
    public float dismountUpOffset = 0.5f;
    public float ledgeGrabDuration = 1.2f;

    [Header("References")]
    public Animator animator;

    private bool isClimbing = false;
    private bool isGrabbingLedge = false;
    private float currentClimbTopY;

    private CharacterController characterController;
    private StarterAssets.ThirdPersonController thirdPersonController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<StarterAssets.ThirdPersonController>();
    }

    void Update()
    {
        if (isGrabbingLedge) return;

        float verticalInput = Input.GetAxis("Vertical");

        // Auto-start climbing when pressing W toward a climbable wall
        if (!isClimbing && verticalInput > 0.1f && CheckForClimbableSurface())
        {
            TryStartClimbing();
        }

        // Climbing movement
        if (isClimbing)
        {
            animator.SetFloat("climbSpeed", verticalInput);
            Vector3 climbMovement = new Vector3(0f, verticalInput * climbSpeed, 0f);
            transform.Translate(climbMovement * Time.deltaTime);

            // Reached top
            if (transform.position.y >= currentClimbTopY && verticalInput > 0f)
            {
                TriggerLedgeGrab();
            }

            // Reached bottom
            if (transform.position.y <= minClimbHeight && verticalInput < 0f)
            {
                StopClimbing();
            }
        }
    }

    bool CheckForClimbableSurface()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out hit, climbCheckDistance, climbableLayer))
        {
            return true;
        }

        return false;
    }

    void TryStartClimbing()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out hit, climbCheckDistance, climbableLayer))
        {
            Debug.Log("Climbable surface detected!");

            isClimbing = true;
            animator.SetBool("isClimbing", true);

            // Disable movement + physics controller
            if (thirdPersonController != null)
                thirdPersonController.enabled = false;

            characterController.enabled = false;

            // Snap to wall
            Vector3 snapPosition = hit.point - direction * 0.4f;
            transform.position = new Vector3(snapPosition.x, transform.position.y, snapPosition.z);

            // Face wall
            Vector3 lookDir = -hit.normal;
            lookDir.y = 0f;
            transform.rotation = Quaternion.LookRotation(lookDir);

            // Record top of object
            currentClimbTopY = hit.collider.bounds.max.y;
        }
    }

    void StopClimbing()
    {
        isClimbing = false;
        animator.SetBool("isClimbing", false);
        animator.SetFloat("climbSpeed", 0f);

        characterController.enabled = true;

        if (thirdPersonController != null)
            thirdPersonController.enabled = true;
    }

    void TriggerLedgeGrab()
    {
        Debug.Log("Triggering ledge grab.");

        isClimbing = false;
        isGrabbingLedge = true;

        animator.SetBool("isClimbing", false);
        animator.SetFloat("climbSpeed", 0f);
        animator.SetTrigger("ledgeGrab");

        if (thirdPersonController != null)
            thirdPersonController.enabled = false;

        characterController.enabled = false;

        // Move forward and upward slightly
        Vector3 forward = transform.forward;
        Vector3 offset = forward * dismountDistance + Vector3.up * dismountUpOffset;
        transform.position += offset;

        // Help physics register ground contact
        transform.position -= Vector3.up * 0.1f;

        StartCoroutine(EndLedgeGrabAfterDelay(ledgeGrabDuration));
    }

    IEnumerator EndLedgeGrabAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        characterController.enabled = true;

        // Wait for one physics frame
        yield return new WaitForFixedUpdate();

        // Extra nudge down to force grounded detection
        transform.position -= Vector3.up * 0.05f;

        yield return null;

        if (thirdPersonController != null)
            thirdPersonController.enabled = true;

        isGrabbingLedge = false;
    }
}
