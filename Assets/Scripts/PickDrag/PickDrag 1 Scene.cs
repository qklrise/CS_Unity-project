using UnityEngine;

// !! ���߿� ����: �������� Ȱ���ϴ� ����̱� ������ �̵��� ����� Collider�� Ȱ��ȭ�ž߸� 3��� ��� �۵��� �� ���� !!

// �������� ��� ������ �������� �����غ� drag �̵�
// �Լ��� �������̽��� ���� ����� ���� �ڵ� ���̰� ��
// ����Ƽ���� ���콺 �巡�׸� ���� ��� ��ġ ��ȭ(����) ���� 3���� ��������,
// �� ����� 2�������� ǥ���� ����� ȭ�鿡���� ���콺 ��ġ ��ǥ�� ��ġ ��ȭ(����) ���� ���� �̵��ؼ� ������ ������.(���ڵ� ������ȣ ����)
// ��� �̵��� ��󿡴� ���̾ �������ְ�, ��ũ��Ʈ�� �ƹ� ������Ʈ�� �־ ����� ����.
// (�ش� ������ �÷��̾� ������Ʈ�� ���� ī�޶� ��ũ��Ʈ�� �־���)

public class PickDrag : MonoBehaviour
{
    public LayerMask pickAbleMask;

    Transform target = null; // drag�� ���� �̵���ų ����� ������ ������ ���� 
    public float DragMoveSpeed = 5.0f;

    Vector3 ReferenceMousePosition = Vector3.zero; // �̵��� �����ϴ� ������ �����ϴ� ����

    void PickObject()
    {
        if(Input.GetMouseButtonDown(0)) // ���콺 ���� Ŭ���� �� ��, �� �巡�� ���� ����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, pickAbleMask))
            {
                target = hit.transform; 
                ReferenceMousePosition = Input.mousePosition; //Ŭ���� �� ��ǥ�� ���������� ����
                target.Translate(Vector3.back * 1.0f,Space.World); // ����� ��Ŭ������ ���õƴٴ� ���� ǥ���ϱ� ���� z ��ǥ�� ������.
            }
        }

        else if ( target != null && Input.GetMouseButton(0)) 
        {
            Vector3 moveDir2D = Input.mousePosition - ReferenceMousePosition; // ����� ȭ��(2D)���� ���� ���콺 ��ǥ ���� �������� ���̸� ���� �̵��� ���Ͱ��� ����
            ReferenceMousePosition = Input.mousePosition; //���ذ��� ���� ���콺 ��ǥ������ ����

            float moveDist = moveDir2D.magnitude; //���� ���Ͱ����� ����� ���� ��ġ���� ����� ���Ͱ���ŭ �̵���Ŵ
            moveDir2D.Normalize();

            if (!Mathf.Approximately(moveDist, 0.0f))
             {
                float delta = DragMoveSpeed * Time.deltaTime;
                if (delta > moveDist) delta = moveDist;
                target.Translate(moveDir2D * delta, Space.World);
                moveDist -= delta;
             }
        }
        
        else if(target != null && !Input.GetMouseButton(0))
        {
            target.Translate(Vector3.forward * 1.0f, Space.World); //drag�� �����⿡ ����� z ��ǥ�� ���� ��ġ�� �ǵ���.
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PickObject();
    }
}
