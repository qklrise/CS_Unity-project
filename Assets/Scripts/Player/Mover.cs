using System;
using System.Collections;
using UnityEngine;

public class Mover : AnimProperty
{
    public Rigidbody Character;
    bool OnGround;
    
    IEnumerator Jump()
    {
        myAnim.SetTrigger("OnJump");
        
        yield return new WaitForSecondsRealtime(0.1f);
        
        Character.AddForce(Vector3.up * 3.0f, ForceMode.Impulse);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        Vector3 moveDir = Character.transform.localRotation * Vector3.forward;
        
        float angle = Vector3.Angle(moveDir, inputDir);
        
        float rotDir = Vector3.Dot(moveDir, Character.transform.localRotation * Vector3.right) < 0.0f ? -1.0f : 1.0f;
        
        float delta = Time.deltaTime * 600.0f;
        
        if (delta > angle) delta = angle;

        transform.Rotate( delta * rotDir * Vector3.up);
        
        myAnim.SetFloat("Speed", inputDir.magnitude);

        
        
        OnGround = Physics.Raycast(transform.position, Vector3.down, 0.2f);
        // 레이저가 무언가에 닿을 때 OnGround를 true로 설정
        if (OnGround && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump());
        }
    }
}