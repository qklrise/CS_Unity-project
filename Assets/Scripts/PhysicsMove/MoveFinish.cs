using UnityEngine;

public class MoveFinish : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!rb.isKinematic && !GameManager.isPuzzle && rb.linearVelocity == Vector3.zero)
        {
            rb.isKinematic = true;
        }
    }
}
