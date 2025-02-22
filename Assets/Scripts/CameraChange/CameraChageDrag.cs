using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class CameraChageDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 oriPosition = Vector3.zero;
    Vector3 oriRotation = Vector3.zero;
    public LayerMask dropAble; // ����� �� �ִ� ���̾� ����
    Transform CameraTrans = null;
    Vector3 CameraDel = Vector3.zero;
    MeshRenderer preDragPoint = null;
    MeshRenderer newDragPoint = null;

    Color ori = default;

    void DragRot(float hitDir, float angleX1, float angleX2, float angleZ1, float angleZ2)
    {
        if (hitDir > 0) transform.rotation = Quaternion.Euler(angleX1, 0, angleZ1);
        else transform.rotation = Quaternion.Euler(angleX2, 0, angleZ2);
    }

    void DropRotMov(Transform trans, Vector3 dir, float angleX, float angleY)
    {
        transform.SetPositionAndRotation(trans.position + dir, Quaternion.Euler(angleX, 0, angleY));
    }
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
        if (CameraTrans == null) CameraTrans = Camera.allCameras[0].transform;
        CameraDel = transform.position - CameraTrans.position;

        float cameraDist = CameraDel.magnitude;// ī�޶���� �Ÿ� ����
        CameraDel.Normalize();

        Vector3 mousePosition = new (Input.mousePosition.x, Input.mousePosition.y, cameraDist);

        Vector3 objPosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        objPosition.y = transform.position.y;
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
                else if (Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.z)) DragRot(hitDir.x, 0, 0, -90.0f, 90.0f);
                else if (Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.x)) DragRot(hitDir.z, 90.0f, -90.0f, 0, 0);
                //���� ���Ͱ����� ���� ������ ����ؼ� �巡���� ������Ʈ�� �˸��� ȸ�������� ����
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
            if (hitDir.y > hitDir.x && hitDir.y > hitDir.z) DropRotMov(hit.transform, Vector3.up, 0, 0);

            else if (Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.z))
            {
                if (hitDir.x > 0) DropRotMov(hit.transform, Vector3.right, 0, -90.0f);
                else DropRotMov(hit.transform, Vector3.left, 0, 90.0f);
            }

            else if (Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.x))
            {
                if (hitDir.z > 0) DropRotMov(hit.transform, Vector3.forward, 90.0f, 0);
                else DropRotMov(hit.transform, Vector3.back, -90.0f, 0);
            }
            // �������� ���� ������Ʈ�� �߽������� �������� ���� �κб��� ������ ����
            // ���� �� ��ǥ ���� ���� �˸��� ��ġ���� ȸ������ ���ؼ� ������
            newDragPoint.material.color = ori;
        }
        else transform.SetPositionAndRotation(oriPosition, Quaternion.Euler(oriRotation));
         // �巡�׸� ���� ���� ������Ʈ�� ���� �� �ִ� ���� �ƴ϶�� ó�� �巡���� ��ġ�� ȸ�������� ���ư�.

        oriPosition = Vector3.zero;
        oriRotation = Vector3.zero;
        CameraTrans = null;
        CameraDel = Vector3.zero;
        preDragPoint = null;
        newDragPoint = null;
        ori = default;
    }
}