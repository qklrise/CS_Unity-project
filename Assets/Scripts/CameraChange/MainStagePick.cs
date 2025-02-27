using UnityEngine;

public class MainStagePick : MonoBehaviour
{
    Vector3 oriPosition = Vector3.zero;
    Vector3 oriRotation = Vector3.zero;
    Vector3 floatPosition = Vector3.zero;
    Vector3 dragOffset = Vector3.zero;
    public LayerMask dropAble; // ����� �� �ִ� ���̾� ����
    Transform CameraTrans = null;
    public PuzzleCamMove2 camMove = null;
    Vector3 mousePosition = Vector3.zero;
    bool IsRotation = false;
    MeshRenderer preDragPoint = null;
    MeshRenderer newDragPoint = null;
    Color ori = default;

    Vector3 WorldMousePointDir()
    {
        Vector3 CameraDel = transform.position - CameraTrans.position;
        float cameraDist = CameraDel.magnitude;// ī�޶���� �Ÿ� ����

        mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        mousePosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        mousePosition.y = floatPosition.y;

        return mousePosition - transform.position;
    }
    public void OnMouseDown() // �巡�� ������ �� ȣ���ϴ� �������̽�
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        if (CameraTrans == null) CameraTrans = Camera.allCameras[0].transform;
        if (camMove == null) camMove = CameraTrans.GetComponentInParent<PuzzleCamMove2>();
        oriPosition = transform.position;
        oriRotation = transform.eulerAngles; //ó�� ��ġ�� ȸ���� ����
        floatPosition = transform.position += Vector3.up * 2.0f;
        dragOffset = WorldMousePointDir();
    }

    public void OnMouseDrag() //�巡�� �߿� ȣ���ϴ� �������̽�    
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        if (!IsRotation)
        {
            Vector3 mousePosVec = WorldMousePointDir() - dragOffset;
            transform.position = transform.position + mousePosVec;
        }
        /*
        float mousePosDist = mousePosVec.magnitude;
        Vector3 mousePosDir = mousePosVec.normalized;
        float delta = Time.deltaTime * 10.0f;
        //if (!Physics.BoxCast(transform.position, transform.lossyScale / 2, mousePosDir, out RaycastHit hit, transform.rotation, delta))
        
        if (delta > mousePosDist) delta = mousePosDist;
        transform.Translate(mousePosDir * delta);
        */
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            IsRotation = !IsRotation; 
            if (camMove.enabled) camMove.enabled = false;
            else camMove.enabled = true;
        }

        if (IsRotation)
        {
            if (Input.GetKeyDown(KeyCode.W)) transform.Rotate(0, 0, 90.0f,Space.World);
            else if (Input.GetKeyDown(KeyCode.S)) transform.Rotate(0, 0, -90.0f, Space.World);
            else if (Input.GetKeyDown(KeyCode.A)) transform.Rotate(-90.0f, 0, 0, Space.World);
            else if (Input.GetKeyDown(KeyCode.D)) transform.Rotate(90.0f, 0, 0, Space.World);
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2.7f, dropAble)) // ��� ���ڿ��� ī�޶� ����(z�� ������)�������� ����, ����� �� �ִ� ���̾����� �Ǵ� 
        {
            newDragPoint = hit.transform.GetComponent<MeshRenderer>(); // ����� �� �ִ� ������Ʈ�� ������Ʈ ����
            if (preDragPoint == newDragPoint) return;
            else
            {
                if (preDragPoint != null) preDragPoint.material.color = ori;//���� ������Ʈ�� ���� ���� ������ �ǵ���
                ori = newDragPoint.material.color;
                newDragPoint.material.color = Color.yellow;
            }
            preDragPoint = hit.transform.GetComponent<MeshRenderer>(); // �� ������Ʈ�� ���� ���� ������Ʈ�� ����
        }
        else if (newDragPoint != null)
        {
            newDragPoint.material.color = ori;
            preDragPoint = null;
            newDragPoint = null;
        }
    }
        
    public void OnMouseUp() // �巡�װ� ���� �� ȣ���ϴ� �������̽�
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) return;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2.7f, dropAble)) // �巡�� ������Ʈ�� �߽������� ī�޶󿡼� �巡�� ������Ʈ �������� �������� ����, ����� �� �ִ� ���̾����� �Ǵ� 
        {
            transform.SetPositionAndRotation(hit.transform.position + Vector3.up, transform.rotation);
            newDragPoint.material.color = ori;
        }
        else transform.SetPositionAndRotation(oriPosition, Quaternion.Euler(oriRotation));
        // �巡�׸� ���� ���� ������Ʈ�� ���� �� �ִ� ���� �ƴ϶�� ó�� �巡���� ��ġ�� ȸ�������� ���ư�.

        oriPosition = Vector3.zero;
        oriRotation = Vector3.zero;
        CameraTrans = null;
        mousePosition = Vector3.zero;
        IsRotation = false;
        camMove.enabled = true;
        preDragPoint = null;
        newDragPoint = null;
        ori = default;
    }
}