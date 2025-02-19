using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 oriPosition = Vector3.zero; 
    Vector3 oriRotation = Vector3.zero;
    public LayerMask dropAble; // ����� �� �ִ� ���̾� ����
    Transform CameraTrans = null;
    Vector3 CameraDel = Vector3.zero;
    MeshRenderer preDragPoint = null;
    MeshRenderer newDragPoint = null;

    Color ori = default;

    public void OnBeginDrag(PointerEventData eventData) // �巡�� ������ �� ȣ���ϴ� �������̽�
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        oriPosition = transform.position;
        oriRotation = transform.eulerAngles; //ó�� ��ġ�� ȸ���� ����
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void OnDrag(PointerEventData eventData) //�巡�� �߿� ȣ���ϴ� �������̽�    
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        if (CameraTrans == null) CameraTrans = Camera.main.GetComponent<Transform>();
        CameraDel = transform.position - CameraTrans.position;

        float cameraDist = CameraDel.magnitude;// ī�޶���� �Ÿ� ����
        CameraDel.Normalize();
        // ���콺 drag�� ��� ��ǥ�� 2����(�⺻ x,y ��ǥ) 3���� ���� ��ȯ�� ���� ī�޶�� Ŭ������ ���� ����� �Ÿ��� �����.
        // ���� �̷��� ����� ������ �ļ���.
        // Camera.main.GetComponent<Transform>().position�� ���� ����ī�޶��� ��ǥ ���� ����

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        // ���� ���콺�� �巡���ϰ� �ִ� ������ x,y�� ������ �־��ְ�, z������ ������ ����� z ��ǥ���� �־���

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objPosition.y = transform.position.y;
        // Camera.main.ScreenToWorldPoint()�� 2d ȭ�� �� ���͸� ��ƼƼ���� ������ 3d ���ͷ� ��������
        // �̶� z��ǥ��ŭ ī�޶� ��� �������� �̵��ؼ� �� ���⿡ �������� ����� ����(���ڵ� ������ȣ ���� �ٶ�)
        // �� ��� ���� 2���� ���͸� �����ؼ� 3���� ���ͷ� ��ȯ
        

        transform.position = objPosition;

        if (Physics.Raycast(transform.position, CameraDel, out RaycastHit hit, 0.9f, dropAble)) // ��� ���ڿ��� ī�޶� ����(z�� ������)�������� ����, ����� �� �ִ� ���̾����� �Ǵ� 
        {
            newDragPoint = hit.transform.GetComponent<MeshRenderer>(); // ����� �� �ִ� ������Ʈ�� ������Ʈ ����
            if (preDragPoint == newDragPoint) return;
            else
            {
                if (preDragPoint != null) preDragPoint.material.color = ori;//���� ������Ʈ�� ���� ���� ������ �ǵ���
                ori = newDragPoint.material.color;
                newDragPoint.material.color = Color.yellow;

                Vector3 hitDir = hit.point - hit.transform.position; // �������� ���� ������Ʈ�� �߽������� �������� ���� �κб��� ������ ����
                if (hitDir.y > hitDir.x && hitDir.y > hitDir.z) transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.z))
                {
                    if (hitDir.x > 0) transform.rotation = Quaternion.Euler(0, 0, -90.0f);
                    else transform.rotation = Quaternion.Euler(0, 0, 90.0f);
                }
                else if (Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.x))
                {
                    if (hitDir.z > 0) transform.rotation = Quaternion.Euler(90.0f, 0, 0);
                    else transform.rotation = Quaternion.Euler(-90.0f, 0, 0);
                } //���� ���Ͱ����� ���� ������ ����ؼ� �巡���� ������Ʈ�� �˸��� ȸ�������� ����
            }
            preDragPoint = hit.transform.GetComponent<MeshRenderer>(); // �� ������Ʈ�� ���� ���� ������Ʈ�� ����
        }
        else if (newDragPoint != null)
        {
            newDragPoint.material.color = ori;
            preDragPoint = null;
            newDragPoint = null;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData) // �巡�װ� ���� �� ȣ���ϴ� �������̽�
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) return;
        if (Physics.Raycast(transform.position, CameraDel, out RaycastHit hit, 0.9f, dropAble)) // �巡�� ������Ʈ�� �߽������� ī�޶󿡼� �巡�� ������Ʈ �������� �������� ����, ����� �� �ִ� ���̾����� �Ǵ� 
        {
            Vector3 hitDir = hit.point - hit.transform.position;
            if (hitDir.y > hitDir.x && hitDir.y > hitDir.z)
            {
                transform.position = hit.transform.position + Vector3.up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            else if (Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.z))
            {
                if (hitDir.x > 0)
                {
                    transform.position = hit.transform.position + Vector3.right;
                    transform.rotation = Quaternion.Euler(0, 0, -90.0f);
                }
                else
                {
                    transform.position = hit.transform.position + Vector3.left;
                    transform.rotation = Quaternion.Euler(0, 0, 90.0f);
                }
            }

            else if (Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.x))
            {
                if (hitDir.z > 0)
                {
                    transform.position = hit.transform.position + Vector3.forward;
                    transform.rotation = Quaternion.Euler(90.0f, 0, 0);
                }

                else
                {
                    transform.position = hit.transform.position + Vector3.back;
                    transform.rotation = Quaternion.Euler(-90.0f, 0, 0);
                }
            }
            // �������� ���� ������Ʈ�� �߽������� �������� ���� �κб��� ������ ����
            // ���� �� ��ǥ ���� ���� �˸��� ��ġ���� ȸ������ ���ؼ� ������
            newDragPoint.material.color = ori;
        }
        else 
        { 
            transform.position = oriPosition;
            transform.rotation = Quaternion.Euler(oriRotation);
        } // �巡�׸� ���� ���� ������Ʈ�� ���� �� �ִ� ���� �ƴ϶�� ó�� �巡���� ��ġ�� ȸ�������� ���ư�.

        oriPosition = Vector3.zero;
        oriRotation = Vector3.zero;
        CameraTrans = null;
        CameraDel = Vector3.zero;
        preDragPoint = null;
        newDragPoint = null;
        ori = default;
    }
}
