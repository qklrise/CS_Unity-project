using UnityEngine;
using System.Collections;
public class PlayerMove : AnimProperty
{
    public Rigidbody Character;
    
    //------점프할 때 쓰는 변수---------
    bool onGround;
    bool onLanding;
    //-----------------------------
    
    public Transform myModel;
    public float moveSpeed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 modelDir = myModel.localRotation * Vector3.forward;
        float angle = Vector3.Angle(modelDir, inputDir);
        float rotdir = Vector3.Dot(inputDir, myModel.localRotation * Vector3.right) < 0.0f ? -1.0f : 1.0f;

        float delta = Time.deltaTime * 720.0f;
        if (delta > angle) delta = angle;
        myModel.Rotate(Vector3.up * (delta * rotdir));
        myAnim.SetFloat("Speed", inputDir.magnitude);

        /*Vector3 inpuDir = new Vector3(Input.GetAxis("Horizontal"), 0,Input.GetAxis("Vertical"));

        Vector3 modelDir = myModel.localRotation * Vector3.forward;
        float angle = Vector3.Angle(modelDir, inpuDir);
        float rotdir = Vector3.Dot(inpuDir, myModel.localRotation * Vector3.right) < 0.0f ? -1.0f : 1.0f;

        float delta = Time.deltaTime * 720.0f;
        if (delta > angle) delta = angle;
        myModel.Rotate(Vector3.up * delta * rotdir);
        myAnim.SetFloat("Speed", inpuDir.magnitude);*/


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
                transform.Translate(Vector3.forward * Speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * Speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Speed);
            }
        }
        
        if (onLanding)
        {
            Land();
        }
    }

    IEnumerator JumpCheck()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        while (!onLanding)
        {
            onLanding = Physics.Raycast(myModel.position, Vector3.down, 0.1f);
            yield return null;
        }
        onGround = true;
    }
    void Jump()
    {
        onGround = false;
        myAnim.SetTrigger("OnJump");
        Character.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
        
        StartCoroutine(JumpCheck()); 
    }
    
    void Land()
    {
        myAnim.SetTrigger("OnLanding"); // 착지 애니메이션 트리거
        
        StopAllCoroutines();
        onLanding = false;
    }
}

