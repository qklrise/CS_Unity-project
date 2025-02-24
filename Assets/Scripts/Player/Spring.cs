using System;
using System.Collections;
using UnityEngine;

public class Spring : AnimProperty
{
    public Transform myPannel;
    public LayerMask Doll;
    public LayerMask pushLayer;

    /*
    IEnumerator On()
    {
        canForce = false;

        Collider[] Obj = Physics.OverlapBox(transform.position, transform.up * 1.0f);

        foreach (Collider c in Obj)
        {
                c.GetComponent<Rigidbody>().AddForce(Vector3.up * 10.0f);
                yield return null ;
                myAnim.SetTrigger("Using");
                canForce = true;
            
        }
        yield return new WaitForSeconds(2.5f);

    }
    */

    public void OnPush()
    {
        Collider[] list = Physics.OverlapBox(myPannel.position, new Vector3(1, 1, 1), transform.rotation,pushLayer); // 함수가 실행될 때 위에 놓인 것들을 찾음
        foreach(Collider col in list)
        {
            col.GetComponent<Rigidbody>()?.AddForce(Vector3.up * 300.0f); // 찾아진 오브젝트에 릿지드 바디가 있으면 해당 오브젝트를 띄움
        }
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                if ((1 << other.gameObject.layer & Doll) != 0) // 레이어 검사가 안됨 분명 맞게 했는데 무슨짓을 해도 안됨 제발살려주세요제이거보면아무나고쳐주세요..
                {
                    //StartCoroutine(On());
                }
            }
        
    }


    private void OnTriggerStay(Collider other)
    {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if ((1 << other.gameObject.layer & Doll) != 0)
                {
                    myAnim.SetTrigger("Using");
                }
            }
    }


}