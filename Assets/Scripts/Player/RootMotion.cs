using UnityEngine;

public class RootMotion : AnimProperty
{
    public LayerMask crashMask;
    Vector3 deltaPosition = Vector3.zero;
    Quaternion deltaRotation = Quaternion.identity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if(Physics.Raycast(transform.position + new Vector3(0, 0.9f, 0), deltaPosition.normalized, 
            out RaycastHit hit, deltaPosition.magnitude, crashMask))
        {
            deltaPosition = deltaPosition.normalized * hit.distance;
        }

        transform.parent.position += deltaPosition;
        transform.parent.rotation *= deltaRotation;
        deltaPosition = Vector3.zero;
        deltaRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorMove()
    {
        deltaPosition += myAnim.deltaPosition;
        deltaRotation *= myAnim.deltaRotation;
    }
}
