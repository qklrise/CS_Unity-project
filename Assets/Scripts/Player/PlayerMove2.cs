
using UnityEngine;


public class PlayerMove2 : AnimProperty
{
    //------점프할 때 쓰는 변수---------
    bool onGround = true; // 
    bool onJumping = false;

    bool jumpForce = false; //점프하는 힘을 가할지 결정하는 변수

    //-------------------------------

    public Transform myModel;
    public float moveSpeed = 1.0f;
    public Transform cameraTransform;
    Vector3 inputDir = Vector3.zero;
    Rigidbody rb = null; 
    int correctVelocity = 0; // 보정된 벨로시티 값을 저장할 변수

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //키보드 입력

        Move();
        float Speed = 2.0f * Time.deltaTime;

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            onJumping = true;
            jumpForce = true;
            myAnim.SetTrigger("OnJump");
        }
        else if (!onGround) // 공중에 있을 때도 조금씩 이동할 수 있게
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
        bool isMove = inputDir.magnitude != 0; //이동중인지 확인
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized; //카메라의 전방 벡터
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;   //카메라의 오른쪽 벡터
            Vector3 moveDir = lookForward * inputDir.y + lookRight * inputDir.x; //이동 방향 설정

            Quaternion viewRot = Quaternion.LookRotation(moveDir.normalized); //이동 방향으로 회전

            myModel.transform.rotation = Quaternion.Lerp(myModel.transform.rotation, viewRot, Time.deltaTime * 20.0f); //모델 회전

            myAnim.SetFloat("Speed", moveDir.magnitude); //애니메이션 속도 설정
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
            // 벨로시티가 0이고 떠 있는 때,
            // 벽 옆면에 충돌할 경우에는 벨로시티의 y값 0이 아니기에 if문 안으로 들어오지 않음

            onGround = true; //착지 상태로 판정
            myAnim.SetTrigger("OnLanding"); // jump3 애니메이션 실행
            if (onJumping) onJumping = false; // 점프 상태(점프키를 누른 경우)였을 경우 해제함
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        SetCorrectVelo();
        if (correctVelocity != 0 && onGround)
        {
            // 착지 상태일 때 y축으로 떨어진다면

            onGround = false; // 체공 상태로 판정
            if (!onJumping) myAnim.SetTrigger("OnAir"); 
            // 점프 상태(점프키를 누른 경우)가 아니라면 jump2 애니메이션 실행
        }
    }

    void SetCorrectVelo()
    {
        float velocity = rb.linearVelocity.y; // 벨로시티의 y값 저장함
        correctVelocity = (int)velocity; // 소수점 이하 값을 버림
    }
}