using UnityEngine;

public class MainStagePick : MonoBehaviour
{
    Vector3 oriPosition = Vector3.zero;
    Vector3 oriRotation = Vector3.zero;
    Vector3 floatPosition = Vector3.zero;
    Vector3 dragOffset = Vector3.zero;
    public LayerMask dropAble; // 드랍할 수 있는 레이어 설정
    Transform CameraTrans = null;
    public PuzzleCamMove2 camMove = null;
    Vector3 mousePosition = Vector3.zero;
    bool IsRotation = false;
    MeshRenderer preDragPoint = null;
    MeshRenderer newDragPoint = null;
    Color ori = default;

    Vector3 WorldMousePointDir()
    {
        Vector3 CameraDel = transform.position - CameraTrans.position;
        float cameraDist = CameraDel.magnitude;// 카메라와의 거리 구함

        mousePosition = new(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        mousePosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        mousePosition.y = floatPosition.y;

        return mousePosition - transform.position;
    }
    public void OnMouseDown() // 드래그 시작할 때 호출하는 인터페이스
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        if (CameraTrans == null) CameraTrans = Camera.allCameras[0].transform;
        if (camMove == null) camMove = CameraTrans.GetComponentInParent<PuzzleCamMove2>();
        oriPosition = transform.position;
        oriRotation = transform.eulerAngles; //처음 위치와 회전값 저장
        floatPosition = transform.position += Vector3.up * 2.0f;
        dragOffset = WorldMousePointDir();
    }

    public void OnMouseDrag() //드래그 중에 호출하는 인터페이스    
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        if (!IsRotation)
        {
            Vector3 mousePosVec = WorldMousePointDir() - dragOffset;
            transform.position = transform.position + mousePosVec;
        }
        /*
        float mousePosDist = mousePosVec.magnitude;
        Vector3 mousePosDir = mousePosVec.normalized;
        float delta = Time.deltaTime * 10.0f;
        //if (!Physics.BoxCast(transform.position, transform.lossyScale / 2, mousePosDir, out RaycastHit hit, transform.rotation, delta))
        
        if (delta > mousePosDist) delta = mousePosDist;
        transform.Translate(mousePosDir * delta);
        */
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            IsRotation = !IsRotation; 
            if (camMove.enabled) camMove.enabled = false;
            else camMove.enabled = true;
        }

        if (IsRotation)
        {
            if (Input.GetKeyDown(KeyCode.W)) transform.Rotate(0, 0, 90.0f,Space.World);
            else if (Input.GetKeyDown(KeyCode.S)) transform.Rotate(0, 0, -90.0f, Space.World);
            else if (Input.GetKeyDown(KeyCode.A)) transform.Rotate(-90.0f, 0, 0, Space.World);
            else if (Input.GetKeyDown(KeyCode.D)) transform.Rotate(90.0f, 0, 0, Space.World);
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2.7f, dropAble)) // 녹색 상자에서 카메라 방향(z축 앞으로)레이져를 쏴서, 드랍할 수 있는 레이어인지 판단 
        {
            newDragPoint = hit.transform.GetComponent<MeshRenderer>(); // 드랍할 수 있는 오브젝트의 컴포넌트 저장
            if (preDragPoint == newDragPoint) return;
            else
            {
                if (preDragPoint != null) preDragPoint.material.color = ori;//기존 오브젝트의 색을 원본 색으로 되돌림
                ori = newDragPoint.material.color;
                newDragPoint.material.color = Color.yellow;
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
        
    public void OnMouseUp() // 드래그가 끝날 때 호출하는 인터페이스
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) return;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2.7f, dropAble)) // 드래그 오브젝트의 중심점에서 카메라에서 드래그 오브젝트 방향으로 레이져를 쏴서, 드랍할 수 있는 레이어인지 판단 
        {
            transform.SetPositionAndRotation(hit.transform.position + Vector3.up, transform.rotation);
            newDragPoint.material.color = ori;
        }
        else transform.SetPositionAndRotation(oriPosition, Quaternion.Euler(oriRotation));
        // 드래그를 끝낸 곳이 오브젝트를 놓을 수 있는 곳이 아니라면 처음 드래그한 위치와 회전값으로 돌아감.

        oriPosition = Vector3.zero;
        oriRotation = Vector3.zero;
        CameraTrans = null;
        mousePosition = Vector3.zero;
        IsRotation = false;
        camMove.enabled = true;
        preDragPoint = null;
        newDragPoint = null;
        ori = default;
    }
}