
using UnityEngine;


public class PlayerMove2 : AnimProperty
{
    //------점프할 때 쓰는 변수---------
    bool onGround = true; // 
    bool inputJumpKey = false;

    bool jumpForce = false; //점프하는 힘을 가할지 결정하는 변수

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
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //키보드 입력

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
        bool isMove = inputDir.magnitude != 0; //이동중인지 확인
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized; //카메라의 전방 벡터
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;   //카메라의 오른쪽 벡터
            Vector3 moveDir = lookForward * inputDir.y + lookRight * inputDir.x; //이동 방향 설정

            Quaternion viewRot = Quaternion.LookRotation(moveDir.normalized); //이동 방향으로 회전

            myModel.rotation = Quaternion.Lerp(myModel.rotation, viewRot, Time.deltaTime * 20.0f); //모델 회전

            myAnim.SetFloat("Speed", moveDir.magnitude); //애니메이션 속도 설정
        }
    }

    private void FixedUpdate()
    {
        float Speed = 2.0f * Time.deltaTime;
        if (!onGround) // 공중에 있을 때도 조금씩 이동할 수 있게
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
            onGround = true; //착지 상태로 판정
            myAnim.SetBool("OnLanding",true); // jump3 애니메이션 실행
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
            // 착지 상태일 때 y축으로 떨어진다면
            onGround = false; // 체공 상태로 판정
            if (!inputJumpKey)
            {
                myAnim.SetTrigger("OnAir");
                // 점프 상태(점프키를 누른 경우)가 아니라면 jump2 애니메이션 실행
                jumpCount--;
            }
            if (inputJumpKey) inputJumpKey = false; // 점프 상태(점프키를 누른 경우)였을 경우 해제함
            //StartPos = transform.position;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (myAnim.GetBool("OnLanding")) myAnim.SetBool("OnLanding", false);
    }
}