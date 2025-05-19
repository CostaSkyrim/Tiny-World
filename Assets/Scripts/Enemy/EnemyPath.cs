using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 21f;
    public float stopTime = 1.2f;
    public float rotationSpeed = 2.5f;
    private int targetPoint;
    private float enemySpeed;
    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = 0;
        enemySpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == patrolPoints[targetPoint].position)
        {
            stop();
            changeTarget();
            Invoke("start", stopTime);
        }
        Transform target = patrolPoints[targetPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, enemySpeed * Time.deltaTime);
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
        enemySpeed = 0;
    }
    void start()
    {
        enemySpeed = speed;
    }
}