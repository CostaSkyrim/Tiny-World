using UnityEngine;

public class PushScissors : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 1f;
    private bool canMove = false;

    void Start()
    {
        enabled = false; // Disabled until triggered by interaction
    }

    public void StartMoving()
    {
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                canMove = false;
                Debug.Log("Scissors reached the target position.");
            }
        }
    }
}
