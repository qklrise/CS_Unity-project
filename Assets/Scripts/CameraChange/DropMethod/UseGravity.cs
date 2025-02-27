using UnityEngine;

public class UseGravity : DragState
{
    Rigidbody rb = null;
    float targetDist = 0.0f;
    float preYpos = 0.0f;
    float newYpos = 0.0f;
    float dropDist = 0.0f;
    float dropYpos = 0.0f;
    protected override void DragStartSet()
    {
        preDragPoint = null;
        newDragPoint = null;
    }
    
    protected override void EndDragSet()
    {
        IsRotation = false;
        camMove.enabled = true;

        if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit hit,2.7f, dropAble)) // 드래그 오브젝트의 중심점에서 카메라에서 드래그 오브젝트 방향으로 레이져를 쏴서, 드랍할 수 있는 레이어인지 판단 
        {
            newDragPoint.material.color = ori;
            rb.useGravity = true;
            dropYpos = hit.transform.position.y + 1.0f;
            targetDist = floatYpos - (hit.transform.position.y + 1.0f);
            preYpos = newYpos = floatYpos;
            
        }
        else
        {
            transform.SetPositionAndRotation(dragStartPos, Quaternion.Euler(dragStartRot));
            ChangeState(State.Stop);
        }
    }

    protected override void EndDragPro()
    {
        newYpos = transform.position.y;
        dropDist = preYpos - newYpos;
        preYpos = newYpos;
        targetDist -= dropDist;

        if (Mathf.Approximately(targetDist, 0.0f) || targetDist < 0.0f)
        {
            transform.position = new(transform.position.x, dropYpos,transform.position.z);
            rb.useGravity = false;
            ChangeState(State.Stop);
        }
    }

    protected override void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        sceanOriPosition = transform.position;
        sceanOriRotation = transform.eulerAngles;
        ChangeState(State.Stop);
    }
}