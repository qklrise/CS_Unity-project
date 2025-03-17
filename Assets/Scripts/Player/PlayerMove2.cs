
using UnityEngine;


public class PlayerMove2 : AnimProperty
{
    //------������ �� ���� ����---------
    public bool onGround { get; private set; } = true; // 
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

            myAnim.SetFloat("Speed", moveDir.magnitude); //�ִϸ��̼� �ӵ� ����
        }
    }

    private void FixedUpdate()
    {
        float Speed = moveSpeed * Time.fixedDeltaTime;
        if (!onGround) // ���߿� ���� ���� ���ݾ� �̵��� �� �ְ�
        {
            transform.Translate(jumpDir * Speed, Space.Self);
            jumpDir = Vector3.zero;
        }

        if (jumpForce)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
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
            col.radius - 0.03f, Vector3.down, out RaycastHit hit, 0.17f))
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