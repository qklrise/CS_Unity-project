using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
public class PlayerMove : AnimProperty
{
    //------점프할 때 쓰는 변수---------
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
        inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //키보드 입력

        Move();
        

        onGround = Physics.Raycast(myModel.position, Vector3.down, 0.1f);
        // 레이저가 바닥에 닿을 때 onGround를 true로 설정
        
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (!onGround) // 공중에 있을 때도 조금씩 이동할 수 있게
        {
            float Speed = 2.0f * Time.deltaTime;
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(myModel.forward * Speed, Space.Self);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(myModel.forward * Speed, Space.Self);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(myModel.forward * Speed, Space.Self);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(myModel.forward * Speed, Space.Self);
            }

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

    private void Move()
    {
        bool isMove =  inputDir.magnitude !=0; //이동중인지 확인
        if(isMove)
        {
            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized; //카메라의 전방 벡터
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;   //카메라의 오른쪽 벡터
            Vector3 moveDir = lookForward * inputDir.y + lookRight * inputDir.x; //이동 방향 설정
        
            Quaternion viewRot = Quaternion.LookRotation(moveDir.normalized); //이동 방향으로 회전

            myModel.transform.rotation = Quaternion.Lerp(myModel.transform.rotation, viewRot, Time.deltaTime * 20.0f); //모델 회전

            myAnim.SetFloat("Speed", moveDir.magnitude); //애니메이션 속도 설정
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
        } // 땅에 닿을 때 까지 레이저로 확인
        onGround = true; // 땅에 닿은 시점에 while문에서 나오고 onGround를 true로

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
        myAnim.SetTrigger("OnLanding"); // 착지 애니메이션 트리거
        
        StopAllCoroutines();
        onLanding = false;
        onJumping = false;
        
        waitTime = 0.0f;
    }

    private void FixedUpdate()
    {
        if (jumpForce)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 300.0f * Time.deltaTime, ForceMode.Impulse);
            jumpForce = false;  
        }
            
    }
}

