
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
       
        if (jumpCount != 0 && Input.GetKeyDown(KeyCode.Space))
        {
            inputJumpKey = true;
            jumpForce = true;
            jumpCount--;
            myAnim.SetTrigger("OnJump");
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
        float Speed = 2.0f * Time.deltaTime;
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
            transform.Translate(jumpDir * Speed, Space.Self);
            jumpDir = Vector3.zero;
        }

        if (jumpForce)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
            jumpForce = false;
        }

        Collider[] overlapCol = Physics.OverlapSphere(transform.position + Vector3.up * (col.radius - 0.1f), col.radius - 0.03f);

        if (!onGround && overlapCol.Length > 1)
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

        else if (overlapCol.Length == 1 && onGround)
        {
            Debug.Log(overlapCol.Length);
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