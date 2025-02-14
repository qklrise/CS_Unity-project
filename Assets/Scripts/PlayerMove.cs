using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Transform player;
    public Transform cameraArm;
    public float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = 10.0f ;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAround();

    }
    void Move() // 캐릭터 움직임 구현 필요
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward * Time.deltaTime * moveSpeed);
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(-transform.forward * Time.deltaTime * moveSpeed);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(-transform.right * Time.deltaTime * moveSpeed);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * Time.deltaTime * moveSpeed);
        }
    }

    private void LookAround() //마우스로 화면 돌려기
    {
        // 마우스 움직임을 Vector2로 저장
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        // 카메라 움직임을 Vector3로 저장
        Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
        // 마우스가 올라가면 카메라가 내려감 (마우스로 바라보는 곳을 바라봄)
        float x = cameraAngle.x - mouseDelta.y;
        // 카메라가 x축으로 돌면서 바닥에 들어가는걸 방지 최대값과 최소값 설정
        if(x < 180.0f)
        {
            x = Mathf.Clamp(x, -1.0f, 70.0f);
        }
        else
        {
            x = Mathf.Clamp(x, 325f, 361f);
        }
        //카메라 트랜스폼 값 설정
        cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    }
}

