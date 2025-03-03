using UnityEngine;

public class UseGravity : DragState
{
    protected Rigidbody rb = null;
    protected float targetDist = 0.0f;
    protected float preYpos = 0.0f;
    protected float newYpos = 0.0f;
    protected float dropDist = 0.0f;
    protected float dropYpos = 0.0f;

    protected override void EndDragSet()
    {
        IsRotation = false;
        camMove.enabled = true;

        if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit hit,2.7f, dropAble)) 
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