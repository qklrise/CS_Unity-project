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
                //on = true; // foreach �� �ȿ��� on ���� �ٲ��ָ�, ��ư ���� �ƴ� ������ eŰ�� ���� ��,
                //������ ��ư ������ �ű� �� ���� ���� ��ȯ���ڸ��� �������� �۵���
            }
            on = true; // on ���� foreach �� �ȿ��� on ���� �ٲ��ָ� �׷� ������ ����
            yield return null;
        }
    }

}
