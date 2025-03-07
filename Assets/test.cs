using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
public class test : AnimProperty
{
    //------������ �� ���� ����---------
    bool onGround;
    bool onLanding;
    bool onJumping;
    private bool jumpForce = false;

    float waitTime = 0.0f;
    //-------------------------------

    public Transform myModel;
    public float moveSpeed = 1.0f;
    public Transform cameraTransform;
    Vector3 inputDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Ű���� �Է�

        Move();

        onGround = Physics.Raycast(myModel.position, Vector3.down, 0.1f);
        // �������� �ٴڿ� ���� �� onGround�� true�� ����

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (!onGround) // ���߿����� ���� ������� �̵�
        {
            AirMove();

            if (!onJumping)
            {
                StartCoroutine(OnAirCheck());
                waitTime = 1000000.0f;
            }
        }

        if (onLanding)
        {
            Land();
        }
    }

    private void AirMove()
    {
        bool isMove = inputDir.magnitude != 0; // �̵� ������ Ȯ��
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized; // ī�޶� ���� ����
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;   // ī�޶� ������ ����
            Vector3 moveDir = (lookForward * inputDir.y + lookRight * inputDir.x).normalized; // �̵� ���� ����

            transform.position += moveDir * moveSpeed * Time.deltaTime; // ������ �ӵ��� �̵�
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
    IEnumerator OnAirCheck()
    {
        yield return new WaitForSeconds(waitTime);

        myAnim.SetTrigger("OnAir");
        StartCoroutine(JumpCheck());
    }

    IEnumerator JumpCheck()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        while (!onLanding)
        {
            onLanding = Physics.Raycast(myModel.position, Vector3.down, 0.1f);
            yield return null;
        } // ���� ���� �� ���� �������� Ȯ��
        onGround = true; // ���� ���� ������ while������ ������ onGround�� true��

    }
    void Jump()
    {
        onJumping = true;
        onGround = false;
        jumpForce = true;
        myAnim.SetTrigger("OnJump");


        StartCoroutine(JumpCheck());
    }

    void Land()
    {
        myAnim.SetTrigger("OnLanding"); // ���� �ִϸ��̼� Ʈ����

        StopAllCoroutines();
        onLanding = false;
        onJumping = false;

        waitTime = 0.0f;
    }

    private void FixedUpdate()
    {
        if (jumpForce)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5.0f * Time.deltaTime, ForceMode.Impulse);
            jumpForce = false;
        }

    }
}

