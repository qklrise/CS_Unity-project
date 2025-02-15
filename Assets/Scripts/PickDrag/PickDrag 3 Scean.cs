using UnityEngine;
using UnityEngine.EventSystems;

// !! ���߿� ����: �������� Ȱ���ϴ� ����̱� ������ �̵��� ����� Collider�� Ȱ��ȭ�ž߸� 3��� ��� �۵��� �� ���� !!

// ����Ƽ�� ����� �������̽��� �̿��� ���
// using UnityEngine.EventSystems�� �����ؾ� ��
// 2�� ����� �������̽��� �Լ��� ���̸� �����ϸ� �ڵ� ������ ������.
// ���콺 �Է��� �޴� ���� �� ������ 2�� ����� ����, 3���� �پ��� �Է� ��ġ�� ���� �� �ִ���(�����е� ��)
// ��� ���� ��ũ��Ʈ�� �߰������ ��
// ī�޶� ������ ���� ����� z ��ǥ ���� ���� ������.
// 3���� �߰��� ī�޶� Physics Raycaster ������Ʈ �߰��ϰ�, Hierarchy�� EventSyetem ������Ʈ �߰��ؾ� �۵���(���ڵ� ���� �ڷ� �����ٶ�)
// 2���� OnMouseDown()�Լ��� Ŭ���� ȣ��Ǽ� Ŭ�����ڸ��� ����������, 3���� OnBeginDrag�� �ܼ� Ŭ���ϸ� ȣ���� �� �ǰ� �巡�װ� ���۵� �� ȣ��.
// �׷��� 2���� ����Ŭ���� �����ϰ� 3���� �������� ����.

public class PickDrag3Scean: MonoBehaviour,IBeginDragHandler, IDragHandler,IEndDragHandler 
{
    Vector3 oriPosition = Vector3.zero;
    Vector3 fromCameraVector = Vector3.zero;

    public void OnBeginDrag(PointerEventData eventData) // �巡�� ������ �� ȣ���ϴ� �������̽�
    {
        transform.Translate(Vector3.back * 1.0f, Space.World); //������ ����� �����ϱ� ���� ���� z������ �ű�
        if (oriPosition == Vector3.zero) oriPosition = transform.position;
    }
    public void OnDrag(PointerEventData eventData) //�巡�� �߿� ȣ���ϴ� �������̽�
    {
        fromCameraVector = Camera.main.GetComponent<Transform>().position - oriPosition;
        float cameraDist = fromCameraVector.magnitude;

        // ���콺 drag�� ��� ��ǥ�� 2����(�⺻ x,y ��ǥ) 3���� ���� ��ȯ�� ���� ī�޶�� Ŭ������ ���� ����� �Ÿ��� �����.
        // ���� �̷��� ����� ������ �ļ���.
        // Camera.main.GetComponent<Transform>().position�� ���� ����ī�޶��� ��ǥ ���� ����

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        // ���� ���콺�� �巡���ϰ� �ִ� ������ x,y�� ������ �־��ְ�, z������ ������ ����� z ��ǥ���� �־���

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // Camera.main.ScreenToWorldPoint()�� 2d ȭ�� �� ���͸� ��ƼƼ���� ������ 3d ���ͷ� ��������
        // �̶� z��ǥ��ŭ ī�޶� ��� �������� �̵��ؼ� �� ���⿡ �������� ����� ����(���ڵ� ������ȣ ���� �ٶ�)
        // �� ��� ���� 2���� ���͸� �����ؼ� 3���� ���ͷ� ��ȯ
        // ���� �ø��� ������ ��� ī�޶� ȸ�� ���� (0,0,0)���� ī�޶� z�� �������� ��� �־
        // z�࿡ ������ ����� ��������� �巡���ϴ� ���� �� �巡�� �ϴ� ���ȿ��� z ��ǥ�� ������ ������.

        transform.position = objPosition;
    }

    public void OnEndDrag(PointerEventData eventData) // �巡�װ� ���� �� ȣ���ϴ� �������̽�
    {
        //if() oriPosition = Vector3.zero; //Ư�� ���ǿ��� �������� �ʱ�ȭ �ؾ���.
        //�� �������� �巡���� ������ �������� �ٲٸ� ������ ����, ������Ʈ�� ������ �ʴ� ��ġ�� �̵���.(�ַ� z��ǥ�� �����Ͽ� ���� �����)

        transform.Translate(Vector3.forward * 1.0f, Space.World); // drag�� ���� ������ z��ǥ�� �ǵ���.
    }
}
