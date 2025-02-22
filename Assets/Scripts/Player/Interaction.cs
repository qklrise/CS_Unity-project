using System.Collections;
using UnityEngine;

public class Interaction : AnimProperty
{
    bool getKey = false;
    bool hasKey = false;
    
    public LayerMask Key;
    public LayerMask Door;

    public GameObject KeyObj;

    public GameObject KeySlot;
    
    public GameObject DoorKeySlot;
    
    IEnumerator GetKey1()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 dir = (KeySlot.transform.position - KeyObj.transform.position).normalized; // 방향 벡터 정규화
        float dist = Vector3.Distance(KeySlot.transform.position, KeyObj.transform.position); // 거리 계산

        while (dist > 0.01f) // 0이 아니라, 어느 정도 가까워질 때까지 반복
        {
            float delta = 0.2f * Time.deltaTime; // 이동 속도 조절
            KeyObj.transform.position = Vector3.MoveTowards(KeyObj.transform.position, KeySlot.transform.position, delta); // MoveTowards의 대상으로 이동
            dist = Vector3.Distance(KeySlot.transform.position, KeyObj.transform.position); // 거리 갱신
            yield return null;
        }

        KeyObj.transform.position = KeySlot.transform.position; // 이동이 끝난 뒤에 정확한 위치로 스냅
        KeyObj.transform.rotation = KeySlot.transform.rotation; // 이동이 끝난 뒤에 회전도 맞춤

        getKey = false; // 딱 한 번만 작동되게
        KeyObj.transform.SetParent(KeySlot.transform); // 이동이 끝난 뒤엔 계속 플레이어를 따라다니게
    }

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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasKey && getKey) StartCoroutine(GetKey1()); // 플레이어의 상호작용 가능 범위 내에 있을 때 열쇠와 상호작용 시 hasKey = true
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if ((1 << other.gameObject.layer & Key) != 0)
            {
                KeyInteract();
            }

            if ((1 << other.gameObject.layer & Door) != 0)
            {
                StartCoroutine(UseKey());
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if ((1 << other.gameObject.layer & Key) != 0)
            {
                KeyInteract();
            }
            if ((1 << other.gameObject.layer & Door) != 0)
            {
                StartCoroutine(UseKey());
            }
        }
    }

    void KeyInteract()
    {
        myAnim.SetTrigger("OnCatch"); //잡는 애니메이션
        KeyObj.transform.SetParent(null); // 열쇠 오브젝트의 부모를 끊어서 더이상 아무런 영향도 안 받게 해줌
                
        getKey = true;
        hasKey = true;
    }
}
