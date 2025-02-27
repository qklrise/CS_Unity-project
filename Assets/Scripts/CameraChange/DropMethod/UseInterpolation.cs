using UnityEngine;

public class UseInterpolation : DragState
{
    [SerializeField] float dropSpeed = 1.0f;
    [SerializeField] float smoothSpeed = 2.0f;
    float targetY,posY = 0.0f;
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
                oriRotation = transform.eulerAngles; //처음 위치와 회전값 저장
                floatPosition = transform.position += Vector3.up * 2.0f;
                break;

            case State.Drop:
                if (Physics.Raycast(GridMouse, Vector3.down, 2.7f, dropAble)) // 드래그 오브젝트의 중심점에서 카메라에서 드래그 오브젝트 방향으로 레이져를 쏴서, 드랍할 수 있는 레이어인지 판단 
                {
                    newDragPoint.material.color = ori;
                    posY = targetY = transform.position.y;
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
                Debug.DrawLine(GridMouse, GridMouse + Vector3.down * 2.7f);
                if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit hit, 2.7f, dropAble)) // 녹색 상자에서 카메라 방향(z축 앞으로)레이져를 쏴서, 드랍할 수 있는 레이어인지 판단 
                {
                    newDragPoint = hit.transform.GetComponent<MeshRenderer>(); // 드랍할 수 있는 오브젝트의 컴포넌트 저장
                    if (preDragPoint == newDragPoint) return;
                    else
                    {
                        if (preDragPoint != null) preDragPoint.material.color = ori;//기존 오브젝트의 색을 원본 색으로 되돌림
                        ori = newDragPoint.material.color;
                        newDragPoint.material.color = Color.yellow;
                        transform.position = hit.transform.position + Vector3.up * 3.0f;
                    }
                    preDragPoint = hit.transform.GetComponent<MeshRenderer>(); // 새 오브젝트의 값을 기존 오브젝트로 저장
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