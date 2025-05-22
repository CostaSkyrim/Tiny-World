using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fallDamage : MonoBehaviour
{
    private float fallStartY;
    private bool isFalling;
    public float fallThreshold = 8f;
    private Vector3 lastPosition;
    private float velocity;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        velocity = (transform.position.y - lastPosition.y) / Time.deltaTime;
        CheckFalling();
        lastPosition = transform.position;
    }

    private void CheckFalling()
    {
        // Start tracking fall if moving downward
        if (velocity < -0.1f && !isFalling)
        {
            isFalling = true;
            fallStartY = transform.position.y;
        }

        // Check for landing
        if (velocity >= -0.1f && isFalling)
        {
            float fallDistance = fallStartY - transform.position.y;
            if (fallDistance > fallThreshold)
            {
                GameManager.Instance.KillPlayer("Fell from height");
            }
            isFalling = false;
        }
    }
}