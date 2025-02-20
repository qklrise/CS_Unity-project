using UnityEngine;
using System.Collections;
public class PlayerMove : AnimProperty
{
    public Rigidbody Character;
    bool OnGround;

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
        myModel.Rotate(Vector3.up * delta * rotdir);
        myAnim.SetFloat("Speed", inputDir.magnitude);

        /*Vector3 inpuDir = new Vector3(Input.GetAxis("Horizontal"), 0,Input.GetAxis("Vertical"));

        Vector3 modelDir = myModel.localRotation * Vector3.forward;
        float angle = Vector3.Angle(modelDir, inpuDir);
        float rotdir = Vector3.Dot(inpuDir, myModel.localRotation * Vector3.right) < 0.0f ? -1.0f : 1.0f;

        float delta = Time.deltaTime * 720.0f;
        if (delta > angle) delta = angle;
        myModel.Rotate(Vector3.up * delta * rotdir);
        myAnim.SetFloat("Speed", inpuDir.magnitude);*/


        OnGround = Physics.Raycast(transform.position, Vector3.down, 0.2f);
        // 레이저가 무언가에 닿을 때 OnGround를 true로 설정
        if (OnGround && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump());
        }
    }
    IEnumerator Jump()
    {
        myAnim.SetTrigger("OnJump");
        
        yield return new WaitForSecondsRealtime(0.1f);
        
        Character.AddForce(Vector3.up * 3.0f, ForceMode.Impulse);
    }
}

