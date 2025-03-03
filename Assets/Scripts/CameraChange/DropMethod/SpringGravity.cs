using UnityEngine;

public class SpringGravity : UseGravity
{
    BoxCollider boxCol = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void EndDragSet()
    {
        IsRotation = false;
        camMove.enabled = true;

        if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit hit, 2.7f, dropAble)) 
        {
            newDragPoint.material.color = ori;
            rb.useGravity = true;
            dropYpos = hit.transform.position.y + 1.0f;
            targetDist = floatYpos - (hit.transform.position.y + 1.0f);
            preYpos = newYpos = floatYpos;
            boxCol.isTrigger = true;
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
            boxCol.isTrigger = false;
            transform.position = new(transform.position.x, dropYpos, transform.position.z);
            rb.useGravity = false;
            ChangeState(State.Stop);
        }
    }

    protected override void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (boxCol == null) boxCol = GetComponent<BoxCollider>();
        rb.useGravity = false;
        sceanOriPosition = transform.position;
        sceanOriRotation = transform.eulerAngles;
        ChangeState(State.Stop);
    }
}
