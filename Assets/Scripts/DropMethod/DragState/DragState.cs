using UnityEngine;

public class DragState : StopState
{
    protected Vector3 dragStartPos = Vector3.zero; //�巡�� ������ �� ��ġ ����
    protected Vector3 dragStartRot = Vector3.zero; //�巡�� ������ �� ȸ�� ����
    public LayerMask dropAble; // ����� �� �ִ� �ٴ� ���̾�
    public Transform puzzleCameraArm; //���� ķ �� transform ����
    protected float floatYpos = 0.0f; //�巡�� �� ��� y ��ǥ
    Vector3 GridMouse = Vector3.zero; // ���� ���콺 ��ġ ����
    protected bool canDrop = false;
    protected Color ori = default; //���� ���� 
    protected float rayHitTranY = 0.0f;

    protected override void OnDragSet()
    {
        dragStartPos = transform.position;
        dragStartRot = transform.eulerAngles; // �巡�� ������ ���� ��ġ�� ȸ�� ������ ���� 
        transform.position += Vector3.up * floatDist; // �巡���� ������Ʈ�� ���
        floatYpos = transform.position.y; // ��� y��ǥ�� ������
        // PuzzleCamMove ������Ʈ ���� ����
        if (!GameManager.isDrag) GameManager.isDrag = true;
        // static ���� GameManager.isDrag�� ����, drag ���·� ����.
        if (floatDist != standardFloatDist) floatDist = standardFloatDist;
        // ��� ���̰� ���� ���̿� �ٸ��ٸ� ��� ���̸� ���� ���̷� ����
    }

    protected override void OnDragPro()
    {
        GridMouse = WorldMousePoint();
        //���� ���콺�� ��ġ ������ ����
        
        if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit rayHit, floatDist + 0.7f, dropAble))
        {
            if (rayHit.transform.GetComponent<MeshRenderer>() != null)
            {
                newDragPoint = rayHit.transform.GetComponent<MeshRenderer>();
                if (preDragPoint == newDragPoint) return;
                if (preDragPoint != null) preDragPoint.material.color = ori;
                ori = newDragPoint.material.color;
                // ���� �ٴ��� �� ������ ����

                Vector3 terminalPos = rayHit.transform.position;
                rayHitTranY = rayHit.transform.position.y;
                terminalPos.y = floatYpos;
                transform.position = terminalPos;

                if (Physics.BoxCast(terminalPos, new Vector3(0.4f, 0.4f, 0.4f), Vector3.down,
                    out RaycastHit boxHit, Quaternion.identity, floatDist + 0.3f))
                {
                    if ((1 << boxHit.transform.gameObject.layer & dropAble) != 0 || preDragPoint == null)
                    {
                        newDragPoint.material.color = Color.yellow;
                        canDrop = true;
                    }

                    else
                    {
                        newDragPoint.material.color = Color.red;
                        canDrop = false;
                    }
                }
                preDragPoint = newDragPoint;
            }
            else
            {
                if (preDragPoint != null) preDragPoint.material.color = ori;

                Vector3 terminalPos = rayHit.transform.position;
                rayHitTranY = rayHit.transform.position.y;
                terminalPos.y = floatYpos;
                transform.position = terminalPos;

                if (Physics.BoxCast(terminalPos, new Vector3(0.4f, 0.4f, 0.4f), Vector3.down, out RaycastHit boxHit, Quaternion.identity, floatDist + 0.3f))
                {
                    if ((1 << boxHit.transform.gameObject.layer & dropAble) != 0 || preDragPoint == null)
                    {
                        canDrop = true;
                    }

                    else
                    {
                        canDrop = false;
                    }
                }

                preDragPoint = null;
                newDragPoint = null;
            }
        }

        else
        {
            // ���� ���콺 ��ġ���� y�� �������� �������� ���� �� ���� ��ǥ �Ʒ����� ������ ���̾ ���ٸ�
            NotDropable();
        }
    }

    void NotDropable()
    {
        if (preDragPoint != null) preDragPoint.material.color = ori;
        //���� ���콺 Ŀ���� �ִ� �ٴ��� ���� ���� ������ ����
        preDragPoint = null;
        newDragPoint = null;
        //������ ���� �ʱ�ȭ
        canDrop = false;
    }

    Vector3 WorldMousePoint()
    {
        Vector3 CameraDel = transform.position - puzzleCameraArm.position;
        //ī�޶�� �巡���� ��ü ������ ������ ����
        float cameraDist = CameraDel.magnitude;// 
        Vector3 mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        // z ��ǥ�� ī�޶���� �Ÿ��� �ǹ�
        mousePosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        //���� ī�޶󿡼� ȭ�� ���� ��ǥ�� ������� ��ǥ�� �����ϴ� �Լ�
        mousePosition.y = floatYpos;
        // y ��ǥ�� ��� ���̷� ����
        return mousePosition;
        // ���� ���콺 Ŀ�� ��ǥ���� ��ȯ
    }
}
