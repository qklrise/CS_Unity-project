using UnityEngine;

public class DragState : StopState
{
    protected Vector3 dragStartPos = Vector3.zero; //드래그 시작할 때 위치 정보
    protected Vector3 dragStartRot = Vector3.zero; //드래그 시작할 때 회전 정보
    public LayerMask dropAble; // 드랍할 수 있는 바닥 레이어
    public Transform puzzleCameraArm; //퍼즐 캠 암 transform 정보
    protected float floatYpos = 0.0f; //드래그 시 띄울 y 좌표
    Vector3 GridMouse = Vector3.zero; // 현재 마우스 위치 정보
    protected bool canDrop = false;
    protected Color ori = default; //색상 정보 
    protected float rayHitTranY = 0.0f;

    protected override void OnDragSet()
    {
        dragStartPos = transform.position;
        dragStartRot = transform.eulerAngles; // 드래그 시작할 때의 위치와 회전 정보를 저장 
        transform.position += Vector3.up * floatDist; // 드래그한 오브젝트를 띄움
        floatYpos = transform.position.y; // 띄운 y좌표를 저장함
        // PuzzleCamMove 컴포넌트 정보 저장
        if (!GameManager.isDrag) GameManager.isDrag = true;
        // static 변수 GameManager.isDrag를 변경, drag 상태로 설정.
        if (floatDist != standardFloatDist) floatDist = standardFloatDist;
        // 띄울 높이가 기준 높이와 다르다면 띄울 높이를 기준 높이로 변경
    }

    protected override void OnDragPro()
    {
        GridMouse = WorldMousePoint();
        //현재 마우스의 위치 정보를 저장
        
        if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit rayHit, floatDist + 0.7f, dropAble))
        {
            if (rayHit.transform.GetComponent<MeshRenderer>() != null)
            {
                newDragPoint = rayHit.transform.GetComponent<MeshRenderer>();
                if (preDragPoint == newDragPoint) return;
                if (preDragPoint != null) preDragPoint.material.color = ori;
                ori = newDragPoint.material.color;
                // 현재 바닥의 색 정보를 저장

                Vector3 terminalPos = rayHit.transform.position;
                rayHitTranY = rayHit.transform.position.y;
                terminalPos.y = floatYpos;
                transform.position = terminalPos;

                if (Physics.BoxCast(terminalPos, new Vector3(0.4f, 0.4f, 0.4f), Vector3.down,
                    out RaycastHit boxHit, Quaternion.identity, floatDist + 0.3f))
                {
                    if ((1 << boxHit.transform.gameObject.layer & dropAble) != 0 || preDragPoint == null)
                    {
                        newDragPoint.material.color = Color.yellow;
                        canDrop = true;
                    }

                    else
                    {
                        newDragPoint.material.color = Color.red;
                        canDrop = false;
                    }
                }
                preDragPoint = newDragPoint;
            }
            else
            {
                if (preDragPoint != null) preDragPoint.material.color = ori;

                Vector3 terminalPos = rayHit.transform.position;
                rayHitTranY = rayHit.transform.position.y;
                terminalPos.y = floatYpos;
                transform.position = terminalPos;

                if (Physics.BoxCast(terminalPos, new Vector3(0.4f, 0.4f, 0.4f), Vector3.down, out RaycastHit boxHit, Quaternion.identity, floatDist + 0.3f))
                {
                    if ((1 << boxHit.transform.gameObject.layer & dropAble) != 0 || preDragPoint == null)
                    {
                        canDrop = true;
                    }

                    else
                    {
                        canDrop = false;
                    }
                }

                preDragPoint = null;
                newDragPoint = null;
            }
        }

        else
        {
            // 현재 마우스 위치에서 y축 방향으로 레이져를 쐈을 때 일정 좌표 아래까지 지정한 레이어가 없다면
            NotDropable();
        }
    }

    void NotDropable()
    {
        if (preDragPoint != null) preDragPoint.material.color = ori;
        //이전 마우스 커서가 있던 바닥의 색을 원래 색으로 변경
        preDragPoint = null;
        newDragPoint = null;
        //저장한 정보 초기화
        canDrop = false;
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
}
