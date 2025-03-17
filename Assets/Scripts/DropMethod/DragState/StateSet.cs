using UnityEngine;
using UnityEngine.UIElements;

// 드래그로 이동하는 스크립트의 부모 스크립트
public class StateSet : MonoBehaviour
{
    private State myState = State.Create;
    protected Vector3 sceanOriPosition = Vector3.zero; //씬 시작할 때 위치 정보
    protected Vector3 sceanOriRotation = Vector3.zero; //씬 시작할 때 회전 정보
    [SerializeField] protected float standardFloatDist = 2.0f; // 드래그 시 떠있을 기준 높이를 정함
    protected float floatDist = 0.0f; // 드래그 시 띄울 높이를 저장
    protected Rigidbody rb = null;

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
        
    }
    protected virtual void OnDragSet()
    {
        
    }

    protected virtual void EndDragSet()
    {
        
    }

    protected virtual void Reset()
    {
   
    }
    void Rotate()
    {
        EnterRotate();
        RotateMove();
    }

    protected virtual void EnterRotate()
    {
        
    }

    protected virtual void RotateMove()
    {
        
    }

    
    protected virtual void OnDragPro()
    {
        
    }

    protected virtual void EndDragPro() //상속해서 내용을 정할 가상함수 
    {

    }
}