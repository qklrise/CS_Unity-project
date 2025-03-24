using System.Collections;
using UnityEngine;

public class Spring : AnimProperty

// 위에 놓인 오브젝트를 붙이는 스크립트
{
    public LayerMask pushAble;
    public GameObject Panel;

    bool On = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 임시로 시험용으로 해둔 것
        if (Input.GetKey(KeyCode.G)) StartCoroutine(OnPiston());
        if (Input.GetKey(KeyCode.H)) OutPiston();
        // 애니메이션 이벤트로 대체 할 예정
    }

    public IEnumerator OnPiston() // 애니메이션 이벤트로 호출하는 함수
    {
        myAnim.SetTrigger("Using");
        On = true;
        Collider[] list = Physics.OverlapBox(transform.position + transform.up * 1.5f, new Vector3(0.45f, 0.45f, 0.45f), transform.rotation, pushAble); // 함수가 실행될 때 위에 놓인 것들을 찾음
        // 판넬 방향으로 한칸 이동한 정육면체 모양의 감지 모양을 만듦
        foreach (Collider col in list)
        {
            Transform orgTf = col.GetComponentInParent<Transform>().parent;

            while (On)
            {
                col.GetComponent<Transform>()?.SetParent(Panel.transform);
                yield return null;
            }

            col.GetComponent<Transform>()?.SetParent(orgTf);
        }
    }


    public void OutPiston() // 애니메이션 이벤트로 호출하는 함수
    {
        On = false;
    }

}
