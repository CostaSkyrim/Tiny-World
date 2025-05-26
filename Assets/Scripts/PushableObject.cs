using UnityEngine;

public class PushableObject : MonoBehaviour
{
    public bool canPush = false;
    public float moveSpeed = 2f;

    void Update()
    {
        if (!canPush) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
