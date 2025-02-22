using System;
using System.Collections;
using UnityEngine;

public class Spring : AnimProperty
{
    public Rigidbody key;
    public LayerMask Doll;

    private bool canForce = true;
    IEnumerator On()
    {
        canForce = false;
        
        key.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);

        myAnim.SetTrigger("Using");
        yield return new WaitForSeconds(1.5f);
        canForce = true;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (canForce)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if ((1 << other.gameObject.layer & Doll) != 0) // 레이어 검사가 안됨 분명 맞게 했는데 무슨짓을 해도 안됨 제발살려주세요제이거보면아무나고쳐주세요..
                {
                    StartCoroutine(On());
                }
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (canForce)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if ((1 << other.gameObject.layer & Doll) != 0)
                {
                    StartCoroutine(On());
                }
            }
        }
    }


}