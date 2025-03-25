using UnityEngine;

public class DragState : StopState
{
    protected Vector3 dragStartPos = Vector3.zero; //�巡�� ������ �� ��ġ ����
    protected Vector3 dragStartRot = Vector3.zero; //�巡�� ������ �� ȸ�� ����
    public LayerMask dropAble; // ����� �� �ִ� �ٴ� ���̾�
    public Transform puzzleCameraArm; //���� ķ �� transform ����
    protected float floatYpos = 0.0f; //�巡�� �� ��� y ��ǥ
    protected bool canDrop = false;
    protected Color ori = default; //���� ���� 
    protected float rayHitTranY = 0.0f;
    float rayDistVar = 0f;
    protected float maxRayYAxisVar { get; set; } = 0f;

    protected override void StartSet()
    {
        float maxRayDist = transform.localScale.y * maxRayYAxisVar + floatDist;
        rayDistVar = maxRayDist + 0.5f;
    }
    protected override void OnDragSet()
    {
        dragStartPos = transform.position;
        dragStartRot = transform.eulerAngles; // �巡�� ������ ���� ��ġ�� ȸ�� ������ ���� 
        transform.position += Vector3.up * floatDist; // �巡���� ������Ʈ�� ���
        floatYpos = transform.position.y; // ��� y��ǥ�� ������
        myCol.enabled = false;
        // PuzzleCamMove ������Ʈ ���� ����
        if (!GameManager.isDrag) GameManager.isDrag = true;
        // static ���� GameManager.isDrag�� ����, drag ���·� ����.
        if (floatDist != standardFloatDist) floatDist = standardFloatDist;
        // ��� ���̰� ���� ���̿� �ٸ��ٸ� ��� ���̸� ���� ���̷� ����
    }

    protected override void OnDragPro()
    {
        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
        Vector3 camDir = puzzleCameraArm.position - transform.position;
        float camDist = camDir.magnitude;
        float rayDist = camDist + rayDistVar;
        
        if (Physics.Raycast(ray, out RaycastHit rayHit, Mathf.Infinity, dropAble))
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

                if (Physics.BoxCast(terminalPos + Vector3.up, new Vector3(0.4f, 0.4f, 0.4f), Vector3.down,
                    out RaycastHit boxHit, Quaternion.identity, Mathf.Infinity)) 

                        //floatDist + 1.3f;
                {
                    if ((1 << boxHit.transform.gameObject.layer & dropAble) != 0 || preDragPoint == null)
                    {
                        newDragPoint.material.color = Color.yellow;
                        canDrop = true;
                        DragAlphaTF(true);
                    }

                    else
                    {
                        newDragPoint.material.color = Color.red;
                        canDrop = false;
                        DragAlphaTF(false);
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
                        DragAlphaTF(true);
                    }

                    else
                    {
                        canDrop = false;
                        DragAlphaTF(false);
                    }
                }

                preDragPoint = null;
                newDragPoint = null;
            }
        }

        else
        {
            // ���� ���콺 ��ġ���� y�� �������� �������� ���� �� ���� ��ǥ �Ʒ����� ������ ���̾ ���ٸ�
            if (preDragPoint != null) preDragPoint.material.color = ori;
            //���� ���콺 Ŀ���� �ִ� �ٴ��� ���� ���� ������ ����
            preDragPoint = null;
            newDragPoint = null;
            //������ ���� �ʱ�ȭ
            canDrop = false;
            DragAlphaTF(false);
        }
    }

    protected virtual void DragAlphaTF(bool tf)
    {

    }
}
