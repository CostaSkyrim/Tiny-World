using UnityEngine;

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

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isGrabbingLedge) return;

        // Toggle climb with C key
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isClimbing)
                TryStartClimbing();
            else
                StopClimbing();
        }

        // Climbing movement logic
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
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

    void TryStartClimbing()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out hit, climbCheckDistance, climbableLayer))
        {
            Debug.Log("Climbable surface detected!");

            isClimbing = true;
            characterController.enabled = false;
            animator.SetBool("isClimbing", true);

            // Snap to wall
            Vector3 snapPosition = hit.point - direction * 0.4f;
            transform.position = new Vector3(snapPosition.x, transform.position.y, snapPosition.z);

            // Face wall
            Vector3 lookDir = -hit.normal;
            lookDir.y = 0f;
            transform.rotation = Quaternion.LookRotation(lookDir);

            // Get top Y position of object
            currentClimbTopY = hit.collider.bounds.max.y;
        }
        else
        {
            Debug.Log("No climbable surface detected.");
        }
    }

    void StopClimbing()
    {
        isClimbing = false;
        animator.SetBool("isClimbing", false);
        animator.SetFloat("climbSpeed", 0f);
        characterController.enabled = true;
    }

    void TriggerLedgeGrab()
    {
        Debug.Log("Triggering ledge grab.");
        isClimbing = false;
        isGrabbingLedge = true;

        // Stop climbing animation
        animator.SetBool("isClimbing", false);
        animator.SetFloat("climbSpeed", 0f);
        animator.SetTrigger("ledgeGrab");

        characterController.enabled = false;

        // Move the character forward & up slightly (simulate mount)
        Vector3 forward = transform.forward;
        Vector3 offset = forward * dismountDistance + Vector3.up * dismountUpOffset;
        transform.position += offset;

        StartCoroutine(EndLedgeGrabAfterDelay(ledgeGrabDuration));
    }

    System.Collections.IEnumerator EndLedgeGrabAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        characterController.enabled = true;
        isGrabbingLedge = false;
    }
}
