using UnityEngine;

public class IceMove : MonoBehaviour
{
    public LayerMask iceMask;
    public PlayerMove2 playerMove;
    Rigidbody rb = null;
    bool onIce = false;
    Vector3 iceMoveDir = Vector3.zero;
    [SerializeField] float iceSpeed = 2.0f;
    float iceMoveSpeed = 0f;
    [SerializeField] float maxIceSpeed = 10.0f;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.1f, iceMask))
        {
            if (!onIce) onIce = true;
        }

        else if (onIce)
        {
            onIce = false;
        }

        if (onIce)
        {
            if(playerMove.inputDir != Vector3.zero)
            {
                iceMoveDir = playerMove.myModel.forward;
                iceMoveSpeed = iceSpeed;
            }

            else
            {
                if (!Mathf.Approximately(iceMoveSpeed, 0f)) 
                {
                    if (iceMoveSpeed < iceSpeed) iceSpeed = iceMoveSpeed;
                    iceMoveSpeed -= iceSpeed;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (onIce)
        {
            float delta = iceMoveSpeed * Time.fixedDeltaTime;
            if (rb.linearVelocity.magnitude < maxIceSpeed)
            {
                if (rb.linearVelocity.magnitude + delta > maxIceSpeed) delta = maxIceSpeed - rb.linearVelocity.magnitude;
                rb.linearVelocity += iceMoveDir * delta;
                Debug.Log(rb.linearVelocity.magnitude);
            }
        }
    }
}
