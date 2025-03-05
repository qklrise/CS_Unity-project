using System;
using System.Collections;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public Transform myPannel;
    public LayerMask pushLayer;


    public void OnPush() // 애니메이션 이벤트로 호출하는 함수
    {
        Collider[] list = Physics.OverlapBox(transform.position + new Vector3 (0, 1.0f, 0), new Vector3(0.5f, 1.0f, 0.5f), transform.rotation, pushLayer); // 함수가 실행될 때 위에 놓인 것들을 찾음
        foreach (Collider col in list)
        {
            col.GetComponent<Rigidbody>()?.AddForce(transform.up * 600.0f); // 찾아진 오브젝트에 릿지드 바디가 있으면 해당 오브젝트를 띄움
        }
    }
}