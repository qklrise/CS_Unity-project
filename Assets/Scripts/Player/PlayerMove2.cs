
using UnityEngine;


public class PlayerMove2 : AnimProperty
{
    //------������ �� ���� ����---------
    bool onGround = true; // 
    bool inputJumpKey = false;

    bool jumpForce = false; //�����ϴ� ���� ������ �����ϴ� ����

    //-------------------------------
    public Transform myModel;
    public float moveSpeed = 1.0f;
    Vector3 jumpDir = Vector3.zero;
    public Transform cameraTransform;
    Vector3 inputDir = Vector3.zero;
    int jumpCount = 2;
    Rigidbody rb = null;
    CapsuleCollider col = null;
    Vector3 StartPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //Ű���� �Է�

        Move();
        float Speed = 2.0f * Time.deltaTime;

        if (jumpCount != 0 && Input.GetKeyDown(KeyCode.Space))
        {
            inputJumpKey = true;
            jumpForce = true;
            jumpCount--;
            myAnim.SetTrigger("OnJump");
        }
        else if (!onGround) // ���߿� ���� ���� ���ݾ� �̵��� �� �ְ�
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

            myAnim.SetFloat("Speed", moveDir.magnitude); //�ִϸ��̼� �ӵ� ����
        }
    }

    private void FixedUpdate()
    {
        if (jumpForce)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
            jumpForce = false;
        }

        if (!onGround && Physics.SphereCast(transform.position + Vector3.up * col.radius,
            col.radius, Vector3.down, out RaycastHit hit, 0.1f))
        {
            onGround = true; //���� ���·� ����
            myAnim.SetBool("OnLanding",true); // jump3 �ִϸ��̼� ����
            jumpCount = 2;
         
            /*
            Vector3 terminalPos = transform.position;
            Vector3 jumpVec = terminalPos - StartPos;
            Debug.Log(jumpVec.magnitude);
            */
        }

        else if (!Physics.SphereCast(transform.position + Vector3.up * col.radius,
            col.radius, Vector3.down, out hit, 0.2f) && onGround && !myAnim.GetBool("OnLanding"))
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
            //StartPos = transform.position;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (myAnim.GetBool("OnLanding")) myAnim.SetBool("OnLanding", false);
    }
}