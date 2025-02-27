using UnityEngine;

public class DragState : MonoBehaviour

{
    public State myState = State.Create;
    protected Vector3 oriPosition = Vector3.zero;
    protected Vector3 oriRotation = Vector3.zero;
    protected Vector3 floatPosition = Vector3.zero;
    public LayerMask dropAble; // 드랍할 수 있는 레이어 설정
    protected Transform CameraTrans = null;
    protected PuzzleCamMove camMove = null;
    protected Vector3 mousePosition = Vector3.zero;
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
                
                break;

            case State.Drag:
                
                break;

            case State.Drop:
                
                break;
        }
    }

    public virtual void StateProcess()
    {
        switch (myState)
        {
            case State.Stop:

                break;

            case State.Drag:
                
                break;

            case State.Drop:
                
                break;
        }
    }
    protected Vector3 WorldMousePoint()
    {
        Vector3 CameraDel = transform.position - CameraTrans.position;
        float cameraDist = CameraDel.magnitude;// 카메라와의 거리 구함

        mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        mousePosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        mousePosition.y = floatPosition.y;

        return mousePosition;
    }

    public void OnMouseDown() // 드래그 시작할 때 호출하는 인터페이스
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        ChangeState(State.Drag);
    }

    public void OnMouseUp() // 드래그가 끝날 때 호출하는 인터페이스
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) return;
        ChangeState(State.Drop);
        // 드래그를 끝낸 곳이 오브젝트를 놓을 수 있는 곳이 아니라면 처음 드래그한 위치와 회전값으로 돌아감.
    }
    protected void Start()
    {
        myState = State.Create;
    }

    protected void Update()
    {
        StateProcess();
    }
}