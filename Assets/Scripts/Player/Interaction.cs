using System;
using System.Collections;
using UnityEngine;

public class Interaction : AnimProperty
{
    public LayerMask Key;
    public LayerMask Door;
    public LayerMask Grow;

    public GameObject KeySlot;
    
    GameObject InteractTarget;
    GameObject DoorKeySlot;
    GameObject GrowSlot;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] list = Physics.OverlapBox(transform.position + transform.up * 0.7f + transform.forward * 0.5f, new Vector3(0.4f ,1.4f ,1.0f) * 0.5f, transform.rotation);
            foreach (Collider col in list)
            {
             //----------------------------------------------------------------------------------------------------------------------------------------------------------

                if ((1 << col.gameObject.layer & Key) != 0)
                    // 상호작용 대상이 'key' 일 때
                {
                    InteractTarget = col.gameObject;
                    col.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // 상호작용 성공한 시점에 상호작용한 오브젝트의 움직임을 멈춤
                    myAnim.SetTrigger("Catch"); //잡는 애니메이션

                    GetComponentInParent<PlayerMove2>().enabled = false; // 잡는 동작 중엔 못 움직이게
                    myAnim.SetFloat("Speed", 0.0f);

                }
                //----------------------------------------------------------------------------------------------------------------------------------------------------------

                if (InteractTarget != null && (1 << InteractTarget.gameObject.layer & Key) != 0) // 열쇠를 가지고 있다면
                {
                    if ((1 << col.gameObject.layer & Door) != 0)
                    // 상호작용 대상이 'door' 일 때
                    {
                        DoorKeySlot = col.gameObject;
                        myAnim.SetTrigger("UseKey"); // 열쇠를 쓰는 애니메이션 실행
                    }

                    else if ((1 << col.gameObject.layer & Grow) != 0)

                    {
                        GrowSlot = col.gameObject;
                        myAnim.SetTrigger("UseKey"); // 열쇠를 쓰는 애니메이션 실행
                    }
                }
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------------------
        }
    }
    
    void KeyInteract() // 애니메이션 이벤트로 호출하는 함수
    {
        StartCoroutine(KeyCatch());
    }

    void UseKey() // 애니메이션 이벤트로 호출하는 함수
    {
        if (DoorKeySlot != null) StartCoroutine(UsingKey());
        else if (GrowSlot != null) StartCoroutine(UsingGrowingPosion());
    }

    IEnumerator KeyCatch()
    {
        Collider[] list = Physics.OverlapBox(transform.position + transform.up * 0.7f + transform.forward * 0.5f, new Vector3(0.4f ,1.4f ,1.0f) * 0.5f, transform.rotation, Key);
        foreach (Collider col in list)
        {
            col.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // 애니메이션의 잡는 모션이 나올 때 열쇠가 다시 움직이게 함

            col.gameObject.GetComponent<Rigidbody>().useGravity = false;
            col.gameObject.GetComponent<Collider>().enabled = false;

            float dist = Vector3.Distance(col.transform.position, KeySlot.transform.position); // 거리 계산

            while (dist > 0.035f) // 정확히 0이 아니라 최대한 가까워질 때 까지 반복
            {
                float targetDist = Time.deltaTime * 3.0f;

                col.transform.position = Vector3.Lerp(col.transform.position, KeySlot.transform.position, targetDist);
                col.transform.rotation = Quaternion.Lerp(col.transform.rotation, KeySlot.transform.rotation, targetDist);

                dist = Vector3.Distance(col.transform.position, KeySlot.transform.position); // 남은 거리 갱신
                yield return null;
            }

            col.transform.position = KeySlot.transform.position; // 이동이 끝난 뒤에 정확한 위치로 스냅
            col.transform.rotation = KeySlot.transform.rotation; // 이동이 끝난 뒤에 회전도 맞춤
            col.transform.SetParent(KeySlot.transform); // 이동이 끝난 뒤엔 계속 플레이어를 따라다니게
            col.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // 플레이어의 손에 붙은 뒤로는 못움직이게(손에서 안 떨어지게)

            GetComponentInParent<PlayerMove2>().enabled = true; // 열쇠를 잡는 동작이 끝나면 플레이어가 다시 움직일 수 있게
        }
    }
    IEnumerator UsingKey()
    {
        InteractTarget.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // 문과 상호작용 시 열쇠를 다시 움직일 수 있게 함
        InteractTarget.transform.SetParent(null); // 열쇠가 더이상 플레이어를 안 따라오게


        gameObject.GetComponentInParent<PlayerMove2>().enabled = false; // 모션 중엔 플레이어가 움직이지 못하게
        myAnim.SetFloat("Speed", 0.0f);

        Vector3 dir = (DoorKeySlot.transform.position - InteractTarget.transform.position).normalized;
        float dist = Vector3.Distance(DoorKeySlot.transform.position, InteractTarget.transform.position);
        while (dist > 0.01f) // 0이 아니라 최대한 근접할 때 까지 이동
        {
            float targetDist = Time.deltaTime * 1.4f;
            InteractTarget.transform.position = Vector3.Lerp(InteractTarget.transform.position, DoorKeySlot.transform.position, targetDist);
            InteractTarget.transform.rotation = Quaternion.Lerp(InteractTarget.transform.rotation, DoorKeySlot.transform.rotation, targetDist);
            dist = Vector3.Distance(DoorKeySlot.transform.position, InteractTarget.transform.position);
            yield return null;
        }
        InteractTarget.transform.position = DoorKeySlot.transform.position; // 대략적인 이동이 끝난 후 정확한 위치에 스냅
        InteractTarget.transform.rotation = DoorKeySlot.transform.rotation;
        gameObject.GetComponentInParent<PlayerMove2>().enabled = true; // 모션이 끝나면 움직일 수 있게

        //----------------------------------------------
        // 열쇠를 든 채로 문과 상호작용 시 해야 할 동작
        // 문이 열린다던가 하는 그런것들 실행
        yield return GameTime.GetWait(2.0f);
        DoorKeySlot.GetComponentInParent<Animator>().SetTrigger("Open");
        InteractTarget.transform.SetParent(DoorKeySlot.transform);
        InteractTarget = null;
        DoorKeySlot = null;
        //----------------------------------------------
    }

    IEnumerator UsingGrowingPosion()
    {
        InteractTarget.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // 문과 상호작용 시 열쇠를 다시 움직일 수 있게 함
        InteractTarget.transform.SetParent(null); // 열쇠가 더이상 플레이어를 안 따라오게


        gameObject.GetComponentInParent<PlayerMove2>().enabled = false; // 모션 중엔 플레이어가 움직이지 못하게
        myAnim.SetFloat("Speed", 0.0f);

        Vector3 dir = (GrowSlot.transform.position - InteractTarget.transform.position).normalized;
        float dist = Vector3.Distance(GrowSlot.transform.position, InteractTarget.transform.position);
        while (dist > 0.01f) // 0이 아니라 최대한 근접할 때 까지 이동
        {
            float targetDist = Time.deltaTime * 1.4f;
            InteractTarget.transform.position = Vector3.Lerp(InteractTarget.transform.position, GrowSlot.transform.position, targetDist);
            InteractTarget.transform.rotation = Quaternion.Lerp(InteractTarget.transform.rotation, GrowSlot.transform.rotation, targetDist);
            dist = Vector3.Distance(GrowSlot.transform.position, InteractTarget.transform.position);
            yield return null;
        }
        
        gameObject.GetComponentInParent<PlayerMove2>().enabled = true; // 모션이 끝나면 움직일 수 있게

        //----------------------------------------------
        yield return GameTime.GetWait(2.0f);
        GrowSlot.GetComponentInParent<Animator>().SetTrigger("OnGrow");
        InteractTarget.transform.SetParent(GrowSlot.transform);
        InteractTarget = null;
        GrowSlot = null;
        //----------------------------------------------
    }

    public void SpeedReset()
    {
        myAnim.SetFloat("Speed", 0.0f);
        // 애니메이션이 끝날 때 호출
    }
}


