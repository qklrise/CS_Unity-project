using UnityEngine;

// 드래그로 이동하는 스크립트의 부모 스크립트
public class DragState : MonoBehaviour
{
    public State myState = State.Create;
    protected Vector3 sceanOriPosition = Vector3.zero; //씬 시작할 때 위치 정보
    protected Vector3 sceanOriRotation = Vector3.zero; //씬 시작할 때 회전 정보
    [SerializeField] float standardFloatDist = 2.0f; // 드래그 시 떠있을 기준 높이를 정함
    protected float floatDist = 0.0f; // 드래그 시 띄울 높이를 저장
    protected Rigidbody rb = null;
    protected Vector3 dragStartPos = Vector3.zero; //드래그 시작할 때 위치 정보
    protected Vector3 dragStartRot = Vector3.zero; //드래그 시작할 때 회전 정보
    protected float floatYpos = 0.0f; //드래그 시 띄울 y 좌표
    public LayerMask dropAble; // 드랍할 수 있는 바닥 레이어
    public LayerMask stackAble; // 드래그해서 쌓을 수 있는 오브젝트를 정하는  레이어 
    public LayerMask notDrop; // 드랍할 수 없는 오브젝트를 정하는 레이어
    public Transform puzzleCameraArm; //퍼즐 캠 암 transform 정보
    PuzzleCamMove camMove = null; // PuzzleCamMove 활성화 여부 결정
    protected bool IsRotation = false; // 현재 회전 상태 여부 확인
    protected Vector3 GridMouse = Vector3.zero; // 현재 마우스 위치 정보
    protected MeshRenderer preDragPoint = null; // 이전 마우스 커서가 있던 바닥의 MeshRenderer 정보
    protected MeshRenderer newDragPoint = null; // 현재 마우스 커서가 있던 바닥의 MeshRenderer 정보
    protected Color ori = default; //색상 정보 

    public enum State
    {
        Create, Stop, Drag, Drop
    }

    protected void ChangeState(State s)
    {
        if (s == myState) return;
        myState = s;

        switch (myState)
        {
            case State.Stop:
                StopSet();
                break;

            case State.Drag:
                OnDragSet();
                break;

            case State.Drop:
                EndDragSet();
                break;
        }
    }

    void StateProcess()
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

    private void Start()
    {
        ChangeState(State.Stop);
        sceanOriPosition = transform.position;
        sceanOriRotation = transform.eulerAngles; // 씬 시작할 때 위치와 회전 정보를 저장함
        floatDist = standardFloatDist; // 띄울 높이를 기준 높이로 변경해 줌
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        StartSet(); 
    }

    protected virtual void StartSet() //상속해서 내용을 정할 가상함수
    {

    }
    private void Update() 
    {
        StateProcess(); 
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        // 마우스 우클릭이나 휠버튼으로 꾹 눌러도 상태 변경이 안 되도록 처리
        if (GameManager.isPuzzle && myState == State.Stop) ChangeState(State.Drag);
        // static 변수 isPuzzle을 통해 퍼즐 모드인지 확인
    }

    public void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) return;
        // 마우스 우클릭이나 휠버튼을 떼도 상태 변경이 안 되도록 처리
        if (myState == State.Drag) ChangeState(State.Drop);

    }
    protected virtual void StopSet()
    {
        if(preDragPoint != null) preDragPoint = null;
        if (newDragPoint != null) newDragPoint = null; //변수 초기화
        if(GameManager.isDrag) GameManager.isDrag = false;
        // static 변수 GameManager.isDrag를 변경, drag 상태가 아닌 것으로 설정.
    }
    void OnDragSet()
    {
        dragStartPos = transform.position; 
        dragStartRot = transform.eulerAngles; // 드래그 시작할 때의 위치와 회전 정보를 저장 
        transform.position += Vector3.up * floatDist; // 드래그한 오브젝트를 띄움
        floatYpos = transform.position.y; // 띄운 y좌표를 저장함
        if(camMove == null) camMove = puzzleCameraArm.GetComponent<PuzzleCamMove>();
        // PuzzleCamMove 컴포넌트 정보 저장
        if (!GameManager.isDrag) GameManager.isDrag = true;
        // static 변수 GameManager.isDrag를 변경, drag 상태로 설정.
        if (floatDist != standardFloatDist) floatDist = standardFloatDist;
        // 띄울 높이가 기준 높이와 다르다면 띄울 높이를 기준 높이로 변경
    }

    void EndDragSet()
    {
        if(IsRotation)IsRotation = false;
        // 회전 상태를 안 풀었다면 드래그가 끝날 때 회전 상태가 아닌 것으로 변경
        if(!camMove.enabled) camMove.enabled = true;
        // 회전 상태를 안 풀었다면 드래그가 끝날 때 puzzleCam에 저장한 컴포넌트를 활성화

        EndDragSetRay();
    }

    protected virtual void EndDragSetRay() //상속해서 내용을 정할 가상함수
    {

    }
    void Reset()
    {
        if(Input.GetKeyDown(KeyCode.B))
        transform.SetPositionAndRotation(sceanOriPosition, Quaternion.Euler(sceanOriRotation));
        // b 키를 누르면 씬 생성 시의 위치와 회전값으로 돌아감
    }
    void Rotate()
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
            //회전 상태와 캠이동 컴포넌트를 상태를 변경
        }
    }

    protected virtual void RotateMove()
    {
        if (IsRotation)
        {
            if (Input.GetKeyDown(KeyCode.W)) transform.Rotate(0, 0, 90.0f);
            else if (Input.GetKeyDown(KeyCode.S)) transform.Rotate(0, 0, -90.0f);
            else if (Input.GetKeyDown(KeyCode.A)) transform.Rotate(-90.0f, 0, 0);
            else if (Input.GetKeyDown(KeyCode.D)) transform.Rotate(90.0f, 0, 0);
        }
        // 회전상태일 때 wasd로 회전
    }

    protected virtual void OnDragPro()
    {
        GridMouse = WorldMousePoint();
        //현재 마우스의 위치 정보를 저장
        Debug.DrawLine(GridMouse, GridMouse + Vector3.down * (floatDist + 0.7f), Color.red);
        if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit hit, floatDist + 0.7f, dropAble | stackAble)) 
        {
            // 현재 마우스 위치에서 y축 방향으로 레이져를 쐈을 때 일정 위치에 지정한 레이어가 있다면
            Vector3 terminalPos = hit.transform.position;
            terminalPos.y = floatYpos;
            // 레이저에 적중한 물체의 중심 위치의 좌표를 저장하고, y좌표를 떠 있는 높이로 설정
            if ((1 << hit.transform.gameObject.layer & stackAble) != 0)
            {
                // 적중한 오브젝트가 쌓을 수 있는 오브젝트라면
                if (preDragPoint != null)
                { 
                    preDragPoint.material.color = ori; 
                    preDragPoint = null;
                    //마우스 커서가 있던 바닥의 색을 원래대로 돌림
                }

                transform.position = terminalPos;
                // 저장한 좌표로 오브젝트를 옮김
            }
            else
            {
                newDragPoint = hit.transform.GetComponent<MeshRenderer>();
                // 바닥의 MeshRenderer 정보를 저장
                if (preDragPoint == newDragPoint) return;
                else
                {
                    if (preDragPoint != null) preDragPoint.material.color = ori;
                    //이전 바닥의 정보가 있다면 전 바닥을 색을 원색으로 되돌림
                    ori = newDragPoint.material.color;
                    // 현재 바닥의 색 정보를 저장
                    newDragPoint.material.color = Color.yellow;
                    // 현재 바닥의 색을 노란색으로 변경
                    transform.position = terminalPos;
                    //드래그한 물체의 위치를 레이저가 닿은 위치의 중심의 위로 이동
                    preDragPoint = newDragPoint;
                    //이전 바닥의 정보에 현재 바닥 정보를 입력
                }
            }
        }
        else if (newDragPoint != null)
        {
            // 현재 마우스 위치에서 y축 방향으로 레이져를 쐈을 때 일정 좌표 아래까지 지정한 레이어가 없다면
            preDragPoint.material.color = ori;
            //이전 마우스 커서가 있던 바닥의 색을 원래 색으로 변경
            preDragPoint = null;
            newDragPoint = null;
            //저장한 정보 초기화
        }
    }

    Vector3 WorldMousePoint()
    {
        Vector3 CameraDel = transform.position - puzzleCameraArm.position;
        //카메라와 드래그한 물체 사이의 방향을 구함
        float cameraDist = CameraDel.magnitude;// 
        Vector3 mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        // z 좌표는 카메라와의 거리를 의미
        mousePosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        //현재 카메라에서 화면 상의 좌표를 월드상의 좌표로 변경하는 함수
        mousePosition.y = floatYpos;
        // y 좌표를 띄울 높이로 변경
        return mousePosition;
        // 구한 마우스 커서 좌표값을 반환
    }

    protected virtual void EndDragPro() //상속해서 내용을 정할 가상함수 
    {

    }
}