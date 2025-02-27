using UnityEngine;

public class DragState : MonoBehaviour
{
    public State myState = State.Create;
    protected Vector3 dragStartPos = Vector3.zero;
    protected Vector3 dragStartRot = Vector3.zero;
    protected Vector3 sceanOriPosition = Vector3.zero;
    protected Vector3 sceanOriRotation = Vector3.zero;
    protected float floatYpos = 0.0f;
    public LayerMask dropAble; // 드랍할 수 있는 레이어 설정
    public Transform CameraTrans;
    protected PuzzleCamMove camMove = null;
    protected bool IsRotation = false;
    protected Vector3 GridMouse = Vector3.zero;
    protected MeshRenderer preDragPoint = null;
    protected MeshRenderer newDragPoint = null;
    protected Color ori = default;

    public enum State
    {
        Create, Stop, Drag, Drop
    }

    public virtual void ChangeState(State s)
    {
        if (s == myState) return;
        myState = s;

        switch (myState)
        {
            case State.Stop:
                DragStartSet();
                break;

            case State.Drag:
                OnDragSet();
                break;

            case State.Drop:
                EndDragSet();
                break;
        }
    }

    public virtual void StateProcess()
    {
        switch (myState)
        {
            case State.Stop:
                Reset();
                break;

            case State.Drag:
                Rotate();
                OnDragPro();
                break;

            case State.Drop:
                EndDragPro();
                break;
        }
    }
    protected Vector3 WorldMousePoint()
    {
        Vector3 CameraDel = transform.position - CameraTrans.position;
        float cameraDist = CameraDel.magnitude;// 카메라와의 거리 구함

        Vector3 mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        mousePosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        mousePosition.y = floatYpos;

        return mousePosition;
    }
    protected virtual void DragStartSet()
    {
        preDragPoint = null;
        newDragPoint = null;
    }
    protected virtual void OnDragSet()
    {
        dragStartPos = transform.position;
        dragStartRot = transform.eulerAngles; //처음 위치와 회전값 저장
        transform.position += Vector3.up * 2.0f;
        floatYpos = transform.position.y;
        if(camMove == null) camMove = CameraTrans.GetComponentInParent<PuzzleCamMove>();
    }

    protected virtual void EndDragSet()
    {
        IsRotation = false;
        camMove.enabled = true;

        if (Physics.Raycast(GridMouse, Vector3.down, 2.7f, dropAble)) // 드래그 오브젝트의 중심점에서 카메라에서 드래그 오브젝트 방향으로 레이져를 쏴서, 드랍할 수 있는 레이어인지 판단 
        {
            newDragPoint.material.color = ori;
        }
        else
        {
            transform.SetPositionAndRotation(dragStartPos, Quaternion.Euler(dragStartRot));
            ChangeState(State.Stop);
        }
    }
    void Reset()
    {
        if(Input.GetKeyDown(KeyCode.B))
        transform.SetPositionAndRotation(sceanOriPosition, Quaternion.Euler(sceanOriRotation));
    }
    protected void Rotate()
    {
        EnterRotate();
        RotateMove();
    }

    void EnterRotate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            IsRotation = !IsRotation;
            if (camMove.enabled) camMove.enabled = false;
            else camMove.enabled = true;
        }
    }

    protected virtual void RotateMove()
    {
        if (IsRotation)
        {
            if (Input.GetKeyDown(KeyCode.W)) transform.Rotate(0, 0, 90.0f, Space.World);
            else if (Input.GetKeyDown(KeyCode.S)) transform.Rotate(0, 0, -90.0f, Space.World);
            else if (Input.GetKeyDown(KeyCode.A)) transform.Rotate(-90.0f, 0, 0, Space.World);
            else if (Input.GetKeyDown(KeyCode.D)) transform.Rotate(90.0f, 0, 0, Space.World);
        }
    }

    protected virtual void OnDragPro()
    {
        GridMouse = WorldMousePoint();
        Debug.DrawLine(GridMouse, GridMouse + Vector3.down * 2.7f, Color.red);
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
    }

    protected virtual void EndDragPro()
    {

    }
    public void OnMouseDown() // 드래그 시작할 때 호출하는 인터페이스
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        if (CameraTrans.gameObject.activeInHierarchy && myState == State.Stop) ChangeState(State.Drag);
    }

    public void OnMouseUp() // 드래그가 끝날 때 호출하는 인터페이스
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) return;
        if (myState == State.Drag) ChangeState(State.Drop);
        // 드래그를 끝낸 곳이 오브젝트를 놓을 수 있는 곳이 아니라면 처음 드래그한 위치와 회전값으로 돌아감.
    }
    protected virtual void Start()
    {
        ChangeState(State.Stop);
        sceanOriPosition = transform.position;
        sceanOriRotation = transform.eulerAngles;
    }

    protected void Update()
    {
        StateProcess();
    }
}