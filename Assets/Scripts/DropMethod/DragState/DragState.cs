using UnityEngine;

public class DragState : StopState
{
    protected Vector3 dragStartPos = Vector3.zero; //드래그 시작할 때 위치 정보
    protected Vector3 dragStartRot = Vector3.zero; //드래그 시작할 때 회전 정보
    public LayerMask dropAble; // 드랍할 수 있는 바닥 레이어
    public Transform puzzleCameraArm; //퍼즐 캠 암 transform 정보
    protected float floatYpos = 0.0f; //드래그 시 띄울 y 좌표
    protected bool canDrop = false;
    protected Color ori = default; //색상 정보 
    protected float rayHitTranY = 0.0f;
    float rayDistVar = 0f;
    protected float maxRayYAxisVar { get; set; } = 0f;

    protected override void StartSet()
    {
        float maxRayDist = transform.localScale.y * maxRayYAxisVar + floatDist;
        rayDistVar = maxRayDist + 0.5f;
    }
    protected override void OnDragSet()
    {
        dragStartPos = transform.position;
        dragStartRot = transform.eulerAngles; // 드래그 시작할 때의 위치와 회전 정보를 저장 
        transform.position += Vector3.up * floatDist; // 드래그한 오브젝트를 띄움
        floatYpos = transform.position.y; // 띄운 y좌표를 저장함
        myCol.enabled = false;
        // PuzzleCamMove 컴포넌트 정보 저장
        if (!GameManager.isDrag) GameManager.isDrag = true;
        // static 변수 GameManager.isDrag를 변경, drag 상태로 설정.
        if (floatDist != standardFloatDist) floatDist = standardFloatDist;
        // 띄울 높이가 기준 높이와 다르다면 띄울 높이를 기준 높이로 변경
    }

    protected override void OnDragPro()
    {
        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
        Vector3 camDir = puzzleCameraArm.position - transform.position;
        float camDist = camDir.magnitude;
        float rayDist = camDist + rayDistVar;
        
        if (Physics.Raycast(ray, out RaycastHit rayHit, Mathf.Infinity, dropAble))
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

                if (Physics.BoxCast(terminalPos + Vector3.up, new Vector3(0.4f, 0.4f, 0.4f), Vector3.down,
                    out RaycastHit boxHit, Quaternion.identity, Mathf.Infinity)) 

                        //floatDist + 1.3f;
                {
                    if ((1 << boxHit.transform.gameObject.layer & dropAble) != 0 || preDragPoint == null)
                    {
                        newDragPoint.material.color = Color.yellow;
                        canDrop = true;
                        DragAlphaTF(true);
                    }

                    else
                    {
                        newDragPoint.material.color = Color.red;
                        canDrop = false;
                        DragAlphaTF(false);
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
                        DragAlphaTF(true);
                    }

                    else
                    {
                        canDrop = false;
                        DragAlphaTF(false);
                    }
                }

                preDragPoint = null;
                newDragPoint = null;
            }
        }

        else
        {
            // 현재 마우스 위치에서 y축 방향으로 레이져를 쐈을 때 일정 좌표 아래까지 지정한 레이어가 없다면
            if (preDragPoint != null) preDragPoint.material.color = ori;
            //이전 마우스 커서가 있던 바닥의 색을 원래 색으로 변경
            preDragPoint = null;
            newDragPoint = null;
            //저장한 정보 초기화
            canDrop = false;
            DragAlphaTF(false);
        }
    }

    protected virtual void DragAlphaTF(bool tf)
    {

    }
}
