using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed;
    public int targetPoint;
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
            changeTarget();
        }
        Transform target = patrolPoints[targetPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }


    void changeTarget()
    {
        targetPoint = (targetPoint + 1) % patrolPoints.Length;
    }
}