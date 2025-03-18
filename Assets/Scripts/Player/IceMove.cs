using UnityEngine;

public class IceMove : AnimProperty
{
    public LayerMask iceMask;
    public Transform character;
    Rigidbody rb = null;
    bool onIce = false;
    Vector3 iceMoveVec = Vector3.zero;
    [SerializeField] float iceSpeed = 2.0f;
    [SerializeField] float maxIceSpeed = 10.0f;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.1f, iceMask))
        {
            if (!onIce)
            {
                onIce = true;
                myAnim.SetBool("OnIce", true);
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }

        else if (onIce)
        {
            onIce = false;
            myAnim.SetBool("OnIce", false);
            iceMoveVec = Vector3.zero;
            rb.interpolation = RigidbodyInterpolation.None;
        }

        if (onIce)
        {
            Vector2 inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (inputDir != Vector2.zero)
            {
                Vector3 iceMoveDir = character.forward * Time.deltaTime;
                iceMoveVec += iceMoveDir;
            }

            else
            {
                Vector3 breakDir = 0.7f * iceMoveVec.normalized * Time.deltaTime;
                if (!Mathf.Approximately(iceMoveVec.magnitude, 0f))  //Vector3.Dot(iceMoveDir, breakDir) > 0 &&
                {
                    if (iceMoveVec.magnitude < breakDir.magnitude) breakDir = iceMoveVec;
                    iceMoveVec -= breakDir;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (onIce)
        {
            float delta = 30f * iceSpeed * Time.fixedDeltaTime;
            float velocityLength = rb.linearVelocity.magnitude;
            if (velocityLength < maxIceSpeed)
            {
                if (iceMoveVec.magnitude * delta > maxIceSpeed) delta = maxIceSpeed / iceMoveVec.magnitude;
                rb.linearVelocity = iceMoveVec * delta;
            }
            Debug.Log(velocityLength);
        }
    }
}
