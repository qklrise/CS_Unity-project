using System.Collections;
using UnityEngine;

public class Doll_blue_Sensor : AnimProperty
{
    public LayerMask mask;
    public Transform target;
    public BoxCollider collider;

    bool on;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(Searching());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !GameManager.isPuzzle)
        {
            on = false;
            StartCoroutine(Searching());
        }

    }

    IEnumerator Searching()
    {
        while (!on)
        {
            target = null;
            Collider[] list = Physics.OverlapBox(collider.transform.position, collider.size * 0.5f, collider.transform.rotation, mask);
            foreach (Collider c in list)
            {
                target = c.transform;

                c.GetComponentInParent<Animator>()?.SetTrigger("Using");
                //on = true; // foreach 문 안에서 on 값을 바꿔주면, 버튼 앞이 아닌 곳에서 e키를 누른 뒤,
                //인형을 버튼 앞으로 옮길 때 퍼즐 모드로 전환하자마자 스프링이 작동함
            }
            on = true; // on 값을 foreach 문 안에서 on 값을 바꿔주면 그런 문제가 없음
            yield return null;
        }
    }

}
