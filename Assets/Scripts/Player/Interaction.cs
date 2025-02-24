using System;
using System.Collections;
using UnityEngine;

public class Interaction : AnimProperty
{
    public LayerMask Key;
    public LayerMask Door;

    public GameObject KeySlot;
    
    public GameObject DoorKeySlot;
    
    bool hasKey = false;


    IEnumerator KeyCatch()
    {
        Collider[] list = Physics.OverlapSphere(transform.position, 1.7f, Key);
        foreach (Collider col in list)
        {
            col.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // 애니메이션의 잡는 모션이 나올 때 다시 움직이게 함
            
            col.gameObject.GetComponent<Rigidbody>().useGravity = false;
            col.gameObject.GetComponent<Collider>().enabled = false;
            
            float dist = Vector3.Distance(col.transform.position, KeySlot.transform.position); // 거리 계산
            
            while (dist > 0.035f) // 정확히 0이 아니라 최대한 가까워질 때 까지 반복
            {
                float targetDist = Time.deltaTime * 3.0f;
                
                col.transform.position = Vector3.Lerp(col.transform.position, KeySlot.transform.position, targetDist);
                col.transform.rotation = Quaternion.Lerp(col.transform.rotation, KeySlot.transform.rotation, targetDist);
                
                dist = Vector3.Distance(col.transform.position, KeySlot.transform.position); // 남은 거리 갱신
                /*                
                float delta = 3.0f * Time.deltaTime; // 이동 속도 조절
                col.transform.position = Vector3.MoveTowards(col.transform.position, KeySlot.transform.position, delta); // MoveTowards의 대상으로 이동
                dist = Vector3.Distance(KeySlot.transform.position, col.transform.position); // 거리 갱신
                */
                yield return null;
            }
            
            col.transform.position = KeySlot.transform.position; // 이동이 끝난 뒤에 정확한 위치로 스냅
            col.transform.rotation = KeySlot.transform.rotation; // 이동이 끝난 뒤에 회전도 맞춤
            col.transform.SetParent(KeySlot.transform); // 이동이 끝난 뒤엔 계속 플레이어를 따라다니게

            GetComponentInParent<PlayerMove>().enabled = true; // 동작이 끝나면 다시 움직일 수 있게
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] list = Physics.OverlapSphere(transform.position, 1.7f, Key);
            foreach (Collider col in list)
            {
                col.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // 상호작용 성공한 시점에 움직임을 멈춤
                myAnim.SetTrigger("OnCatch"); //잡는 애니메이션
                
                GetComponentInParent<PlayerMove>().enabled = false; // 잡는 동작 중엔 못 움직이게
            }
        }
    }
    /*
    IEnumerator UseKey() // 열쇠롤 사용
    {
        myAnim.SetTrigger("UseKey"); // 열쇠를 쓰는 애니메이션 실행
        yield return new WaitForSeconds(2.5f);
        KeyObj.transform.SetParent(null); // 더이상 플레이어를 안 따라오게
        gameObject.GetComponent<Mover>().enabled = false;
        Vector3 dir = (DoorKeySlot.transform.position - KeyObj.transform.position).normalized;
        float dist = Vector3.Distance(DoorKeySlot.transform.position, KeyObj.transform.position);

        while (dist > 0.01f)
        {
            float delta = 0.5f * Time.deltaTime;
            KeyObj.transform.position = Vector3.MoveTowards(KeyObj.transform.position, DoorKeySlot.transform.position, delta);
            dist = Vector3.Distance(DoorKeySlot.transform.position, KeyObj.transform.position);
            yield return null;
        }
        
        KeyObj.transform.position = DoorKeySlot.transform.position;
        KeyObj.transform.rotation = DoorKeySlot.transform.rotation;
        
        gameObject.GetComponent<Mover>().enabled = true;
    }
    */

    void KeyInteract()
    {
        StartCoroutine(KeyCatch());
    }
}
