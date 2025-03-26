using System.Collections;
using UnityEngine;

public class Spring : AnimProperty

// ���� ���� ������Ʈ�� ���̴� ��ũ��Ʈ
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
        // �ӽ÷� ��������� �ص� ��
        if (Input.GetKey(KeyCode.G)) StartCoroutine(OnPiston());
        if (Input.GetKey(KeyCode.H)) OutPiston();
        // �ִϸ��̼� �̺�Ʈ�� ��ü �� ����
    }

    public IEnumerator OnPiston() // �ִϸ��̼� �̺�Ʈ�� ȣ���ϴ� �Լ�
    {
        myAnim.SetTrigger("Spring1On");
        On = true;
        Collider[] list = Physics.OverlapBox(transform.position + transform.up * 1.5f, new Vector3(0.45f, 0.45f, 0.45f), transform.rotation, pushAble); // �Լ��� ����� �� ���� ���� �͵��� ã��
        // �ǳ� �������� ��ĭ �̵��� ������ü ����� ���� ����� ����
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


    public void OutPiston() // �ִϸ��̼� �̺�Ʈ�� ȣ���ϴ� �Լ�
    {
        myAnim.SetTrigger("Spring1Off");
        On = false;
    }

}
