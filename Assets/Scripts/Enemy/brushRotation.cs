using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brushRotation : MonoBehaviour
{
    public float rotationSpeed = 200f; // Speed of rotation
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
