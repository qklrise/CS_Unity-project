using UnityEngine;

public class DragState : MonoBehaviour

{
    public State myState = State.Create;
    protected Vector3 oriPosition = Vector3.zero;
    protected Vector3 oriRotation = Vector3.zero;
    protected Vector3 floatPosition = Vector3.zero;
    public LayerMask dropAble; // ����� �� �ִ� ���̾� ����
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
        float cameraDist = CameraDel.magnitude;// ī�޶���� �Ÿ� ����

        mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        mousePosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        mousePosition.y = floatPosition.y;

        return mousePosition;
    }

    public void OnMouseDown() // �巡�� ������ �� ȣ���ϴ� �������̽�
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        ChangeState(State.Drag);
    }

    public void OnMouseUp() // �巡�װ� ���� �� ȣ���ϴ� �������̽�
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) return;
        ChangeState(State.Drop);
        // �巡�׸� ���� ���� ������Ʈ�� ���� �� �ִ� ���� �ƴ϶�� ó�� �巡���� ��ġ�� ȸ�������� ���ư�.
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