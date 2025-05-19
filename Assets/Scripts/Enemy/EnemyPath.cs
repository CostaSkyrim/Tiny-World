using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 21f;
    public float rotationSpeed = 2f;
    public int targetPoint;
    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == patrolPoints[targetPoint].position)
        {
            stop();
            changeTarget();
            Invoke("start", 1f);
        }
        Transform target = patrolPoints[targetPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    void changeTarget()
    {
        targetPoint = (targetPoint + 1) % patrolPoints.Length;
        Vector3 directionToTarget = patrolPoints[targetPoint].position - transform.position;
        targetRotation = Quaternion.LookRotation(directionToTarget);
    }
    void stop()
    {
        speed = 0;
    }
    void start()
    {
        speed = 21f;
    }
}