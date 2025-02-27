using UnityEngine;

public class UseInterpolation : DragState
{
    [SerializeField] float dropSpeed = 1.0f;
    [SerializeField] float smoothSpeed = 2.0f;
    float targetY,posY = 0.0f;
    /*
    public override void ChangeState(State s)
    {
        

            case State.Drop:
                if (Physics.Raycast(GridMouse, Vector3.down, 2.7f, dropAble)) // �巡�� ������Ʈ�� �߽������� ī�޶󿡼� �巡�� ������Ʈ �������� �������� ����, ����� �� �ִ� ���̾����� �Ǵ� 
                {
                    newDragPoint.material.color = ori;
                    posY = targetY = transform.position.y;
                }
                else
                {
                    //transform.SetPositionAndRotation(oriPosition, Quaternion.Euler(oriRotation));
                    ChangeState(State.Stop);
                }
                break;
        }
    }
    */
    public override void StateProcess()
    {
        switch (myState)
        {
            case State.Stop:
                
                break;

            case State.Drag:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    IsRotation = !IsRotation;
                    
                }

                if (IsRotation)
                {
                    if (Input.GetKeyDown(KeyCode.W)) transform.Rotate(0, 0, 90.0f, Space.World);
                    else if (Input.GetKeyDown(KeyCode.S)) transform.Rotate(0, 0, -90.0f, Space.World);
                    else if (Input.GetKeyDown(KeyCode.A)) transform.Rotate(-90.0f, 0, 0, Space.World);
                    else if (Input.GetKeyDown(KeyCode.D)) transform.Rotate(90.0f, 0, 0, Space.World);
                }

                GridMouse = WorldMousePoint();
                Debug.DrawLine(GridMouse, GridMouse + Vector3.down * 2.7f);
                if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit hit, 2.7f, dropAble)) // ��� ���ڿ��� ī�޶� ����(z�� ������)�������� ����, ����� �� �ִ� ���̾����� �Ǵ� 
                {
                    newDragPoint = hit.transform.GetComponent<MeshRenderer>(); // ����� �� �ִ� ������Ʈ�� ������Ʈ ����
                    if (preDragPoint == newDragPoint) return;
                    else
                    {
                        if (preDragPoint != null) preDragPoint.material.color = ori;//���� ������Ʈ�� ���� ���� ������ �ǵ���
                        ori = newDragPoint.material.color;
                        newDragPoint.material.color = Color.yellow;
                        transform.position = hit.transform.position + Vector3.up * 3.0f;
                    }
                    preDragPoint = hit.transform.GetComponent<MeshRenderer>(); // �� ������Ʈ�� ���� ���� ������Ʈ�� ����
                }
                else if (newDragPoint != null)
                {
                    newDragPoint.material.color = ori;
                    preDragPoint = null;
                    newDragPoint = null;
                }
                break;

            case State.Drop:
                
                float temp = dropSpeed;
                targetY -= temp;
                posY = Mathf.Lerp(posY, targetY, Time.deltaTime * smoothSpeed);
                transform.position = new (transform.position.x, posY, transform.position.z);
                Debug.Log("do");
                //ChangeState(State.Stop);
                break;
        }
    }
}