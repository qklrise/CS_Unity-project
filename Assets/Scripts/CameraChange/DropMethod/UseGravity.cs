using UnityEngine;

public class UseGravity : DragState
{

    public override void ChangeState(State s)
    {
        if (s == myState) return;
        myState = s;

        switch (myState)
        {
            case State.Stop:
                oriPosition = Vector3.zero;
                oriRotation = Vector3.zero;
                CameraTrans = null;
                mousePosition = Vector3.zero;
                IsRotation = false;
                GridMouse = Vector3.zero;
                camMove.enabled = true;
                preDragPoint = null;
                newDragPoint = null;
                ori = default;
                break;

            case State.Drag:
                if (CameraTrans == null) CameraTrans = Camera.allCameras[0].transform;
                if (camMove == null) camMove = CameraTrans.GetComponentInParent<PuzzleCamMove>();
                oriPosition = transform.position;
                oriRotation = transform.eulerAngles; //ó�� ��ġ�� ȸ���� ����
                GetComponent<Rigidbody>().useGravity = false;
                floatPosition = transform.position += Vector3.up * 2.0f;
                break;

            case State.Drop:
                if (Physics.Raycast(GridMouse, Vector3.down, 2.7f, dropAble)) // �巡�� ������Ʈ�� �߽������� ī�޶󿡼� �巡�� ������Ʈ �������� �������� ����, ����� �� �ִ� ���̾����� �Ǵ� 
                {
                    newDragPoint.material.color = ori;
                    GetComponent<Rigidbody>().useGravity = true;
                    ChangeState(State.Stop);
                }
                else
                {
                    transform.SetPositionAndRotation(oriPosition, Quaternion.Euler(oriRotation));
                    ChangeState(State.Stop);
                }
                    break;
        }
    }
    
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
                    if (camMove.enabled) camMove.enabled = false;
                    else camMove.enabled = true;
                }

                if (IsRotation)
                {
                    if (Input.GetKeyDown(KeyCode.W)) transform.Rotate(0, 0, 90.0f, Space.World);
                    else if (Input.GetKeyDown(KeyCode.S)) transform.Rotate(0, 0, -90.0f, Space.World);
                    else if (Input.GetKeyDown(KeyCode.A)) transform.Rotate(-90.0f, 0, 0, Space.World);
                    else if (Input.GetKeyDown(KeyCode.D)) transform.Rotate(90.0f, 0, 0, Space.World);
                }

                GridMouse = WorldMousePoint();
                Debug.DrawLine(GridMouse, GridMouse + Vector3.down * 2.7f,Color.red);
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
                break;
        }
    }
}