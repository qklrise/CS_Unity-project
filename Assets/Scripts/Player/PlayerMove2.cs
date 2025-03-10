
using UnityEngine;


public class PlayerMove2 : AnimProperty
{
    //------������ �� ���� ����---------
    bool onGround = true; // 
    bool onJumping = false;

    bool jumpForce = false; //�����ϴ� ���� ������ �����ϴ� ����

    //-------------------------------

    public Transform myModel;
    public float moveSpeed = 1.0f;
    public Transform cameraTransform;
    Vector3 inputDir = Vector3.zero;
    Rigidbody rb = null; 
    int correctVelocity = 0; // ������ ���ν�Ƽ ���� ������ ����

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //Ű���� �Է�

        Move();
        float Speed = 2.0f * Time.deltaTime;

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            onJumping = true;
            jumpForce = true;
            myAnim.SetTrigger("OnJump");
        }
        else if (!onGround) // ���߿� ���� ���� ���ݾ� �̵��� �� �ְ�
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    Speed /= 1.5f;
                transform.Translate(myModel.forward * Speed, Space.Self);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    Speed /= 1.5f;
                transform.Translate(myModel.forward * Speed, Space.Self);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(myModel.forward * Speed, Space.Self);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(myModel.forward * Speed, Space.Self);
            }
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

            myModel.transform.rotation = Quaternion.Lerp(myModel.transform.rotation, viewRot, Time.deltaTime * 20.0f); //�� ȸ��

            myAnim.SetFloat("Speed", moveDir.magnitude); //�ִϸ��̼� �ӵ� ����
        }
    }

    private void FixedUpdate()
    {
        if (jumpForce)
        {
            rb.AddForce(Vector3.up * 6.0f, ForceMode.Impulse);
            jumpForce = false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        SetCorrectVelo();
        if (correctVelocity == 0 && !onGround) 
        {
            // ���ν�Ƽ�� 0�̰� �� �ִ� ��,
            // �� ���鿡 �浹�� ��쿡�� ���ν�Ƽ�� y�� 0�� �ƴϱ⿡ if�� ������ ������ ����

            onGround = true; //���� ���·� ����
            myAnim.SetTrigger("OnLanding"); // jump3 �ִϸ��̼� ����
            if (onJumping) onJumping = false; // ���� ����(����Ű�� ���� ���)���� ��� ������
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        SetCorrectVelo();
        if (correctVelocity != 0 && onGround)
        {
            // ���� ������ �� y������ �������ٸ�

            onGround = false; // ü�� ���·� ����
            if (!onJumping) myAnim.SetTrigger("OnAir"); 
            // ���� ����(����Ű�� ���� ���)�� �ƴ϶�� jump2 �ִϸ��̼� ����
        }
    }

    void SetCorrectVelo()
    {
        float velocity = rb.linearVelocity.y; // ���ν�Ƽ�� y�� ������
        correctVelocity = (int)velocity; // �Ҽ��� ���� ���� ����
    }
}