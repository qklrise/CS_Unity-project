
using UnityEngine;


public class PlayerMove2 : AnimProperty
{
    //------������ �� ���� ����---------
    public bool onGround { get; set; } = true; // 
    bool inputJumpKey = false;

    bool jumpForce = false; //�����ϴ� ���� ������ �����ϴ� ����

    //-------------------------------
    public Transform myModel;
    public float moveSpeed = 3.0f;
    Vector3 jumpDir = Vector3.zero;
    public Transform cameraTransform;
    public Vector3 inputDir { get; private set; } = Vector3.zero;
    int jumpCount = 2;
    Rigidbody rb = null;
    CapsuleCollider col = null;
    float maxSpeed = 1.0f;
    [SerializeField] float jumpPower = 6.0f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //Ű���� �Է�

        Move();

        if (jumpCount != 0 && Input.GetKeyDown(KeyCode.Space))
        {
            inputJumpKey = true;
            jumpForce = true;
            jumpCount--;
            myAnim.SetTrigger("OnJump");
        }

        if (!onGround) // ���߿� ���� ���� ���ݾ� �̵��� �� �ְ�
        {
            if (Input.GetKey(KeyCode.W))
            {
                jumpDir += myModel.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                jumpDir += myModel.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                jumpDir += myModel.forward;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                jumpDir += myModel.forward;
            }
            jumpDir.Normalize();

            float Speed = moveSpeed * Time.deltaTime;
            transform.Translate(jumpDir * Speed, Space.Self);
            jumpDir = Vector3.zero;
        }
        
    }

    private void Move()
    {
        bool isMove = inputDir.magnitude != 0; //�̵������� Ȯ��
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized; //ī�޶��� ���� ����
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;   //ī�޶��� ������ ����
            Vector3 moveDir = lookForward * inputDir.y + lookRight * inputDir.x; //�̵� ���� ����

            Quaternion viewRot = Quaternion.LookRotation(moveDir.normalized); //�̵� �������� ȸ��

            myModel.rotation = Quaternion.Lerp(myModel.rotation, viewRot, Time.deltaTime * 20.0f); //�� ȸ��

            float rootMotionSpeed = moveDir.magnitude;
            rootMotionSpeed = Mathf.Clamp(rootMotionSpeed, 0, maxSpeed);
            myAnim.SetFloat("Speed", rootMotionSpeed); //�ִϸ��̼� �ӵ� ����
        }
    }
    public void SetMaxSpeed(float speed)
    {
        if(maxSpeed != speed) maxSpeed = speed;   
    }
    private void FixedUpdate()
    {
        if (jumpForce)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumpForce = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (onGround) return;
        if (Physics.SphereCast(transform.position + Vector3.up * 0.2f, 
            col.radius - 0.03f, Vector3.down, out RaycastHit hit, 0.3f))
        {
            onGround = true; //���� ���·� ����
            myAnim.SetBool("OnLanding", true); // jump3 �ִϸ��̼� ����
            jumpCount = 2;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (myAnim.GetBool("OnLanding")) myAnim.SetBool("OnLanding", false);
        if (!onGround) return;
        if (!Physics.SphereCast(transform.position + Vector3.up * 0.2f,
            col.radius - 0.03f, Vector3.down, out RaycastHit hit, 0.15f))
        {
            // ���� ������ �� y������ �������ٸ�
            onGround = false; // ü�� ���·� ����
            if (!inputJumpKey)
            {
                myAnim.SetTrigger("OnAir");
                // ���� ����(����Ű�� ���� ���)�� �ƴ϶�� jump2 �ִϸ��̼� ����
                jumpCount--;
            }
            if (inputJumpKey) inputJumpKey = false; // ���� ����(����Ű�� ���� ���)���� ��� ������
        }
    }
}