using UnityEngine;

// !! ���߿� ����: �������� Ȱ���ϴ� ����̱� ������ �̵��� ����� Collider�� Ȱ��ȭ�ž߸� 3��� ��� �۵��� �� ���� !!

// ����Ƽ�� ����� �Լ� OnMouseDrag�� OnMouseUp�� �̿��ϴ� ���
// 1�� ����� ���� �ڵ尡 ������.
// �⺻������ ���콺 �Է¿��� ���� �� �ְ�, ȭ�� ��ġ ���� �ް� �� �� ����
// ��� ���� ��ũ��Ʈ�� �߰������ ��
// ī�޶� ������ ���� ����� z ��ǥ ���� ���� ������.

public class PickDrag2Scean : MonoBehaviour
{
    Vector3 oriPosition = Vector3.zero; //�Ÿ� �������� �Ǵ� ��ǥ�� �����ϴ� ����
    Vector3 fromCameraVector = Vector3.zero; // �������� ī�޶���� �Ÿ��� �����ϴ� ����

    private void OnMouseDown()// ���콺�� ���� �� ȣ���ϴ� �Լ�, ���� ȣ������ �ʾƵ� ���콺�� ���� �� ȣ����.
    {
        transform.Translate(Vector3.back * 1.0f,Space.World); //������ ����� �����ϱ� ���� ���� z������ �ű�
        if(oriPosition == Vector3.zero)oriPosition = transform.position; 
    }
    void OnMouseDrag() // ���콺�� ������ ���� ��� ȣ���ϴ� �Լ�, ���� ȣ������ �ʾƵ� ���콺�� ������ ������ ȣ����.
    {
        fromCameraVector = Camera.main.GetComponent<Transform>().position - oriPosition;
        float cameraDist = fromCameraVector.magnitude; 
        
        // ���콺 drag�� ��� ��ǥ�� 2����(�⺻ x,y ��ǥ) 3���� ���� ��ȯ�� ���� ī�޶�� ����� ù ���ý� �Ÿ��� �����.
        // ���� �̷��� ����� ������ �ļ���.
        // Camera.main.GetComponent<Transform>().position�� ���� ����ī�޶��� ��ǥ ���� ����

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist); 
        // ���� ���콺�� �巡���ϰ� �ִ� ������ x,y�� ������ �־��ְ�, z������ ������ ����� z ��ǥ���� �־���

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // Camera.main.ScreenToWorldPoint()�� 2d ȭ�� �� ���͸� ����Ƽ���� ������ 3d ���ͷ� ��������
        // �̶� z��ǥ��ŭ ī�޶� ��� �������� �̵��ؼ� �� ���⿡ �������� ����� ����(���ڵ� ������ȣ ���� �ٶ�)
        // �� ��� ���� 2���� ���͸� �����ؼ� 3���� ���ͷ� ��ȯ
        // ���� �ø��� ������ ��� ī�޶� ȸ�� ���� (0,0,0)���� ī�޶� z�� �������� ��� �־
        // z�࿡ ������ ����� ��������� �巡���ϴ� ���� �� �巡�� �ϴ� ���ȿ��� z ��ǥ�� ������ ������.

        transform.position = objPosition;
    }

    void OnMouseUp() //���콺�� �� �� ȣ���ϴ� �Լ�. �� drag�� ������ ȣ��
    {
        //if() oriPosition = Vector3.zero; //Ư�� ���ǿ��� �������� �ʱ�ȭ �ؾ���.
        //�� �������� �巡���� ������ �������� �ٲٸ� ������ ����, ������Ʈ�� ������ �ʴ� ��ġ�� �̵���.(�ַ� z��ǥ�� �����Ͽ� ���� �����)

        transform.Translate(Vector3.forward * 1.0f,Space.World); // drag�� ���� ������ z��ǥ�� �ǵ���.
    }
}
