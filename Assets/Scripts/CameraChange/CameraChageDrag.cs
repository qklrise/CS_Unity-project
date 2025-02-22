using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class CameraChageDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 oriPosition = Vector3.zero;
    Vector3 oriRotation = Vector3.zero;
    public LayerMask dropAble; // 드랍할 수 있는 레이어 설정
    Transform CameraTrans = null;
    Vector3 CameraDel = Vector3.zero;
    MeshRenderer preDragPoint = null;
    MeshRenderer newDragPoint = null;

    Color ori = default;

    void DragRot(float hitDir, float angleX1, float angleX2, float angleZ1, float angleZ2)
    {
        if (hitDir > 0) transform.rotation = Quaternion.Euler(angleX1, 0, angleZ1);
        else transform.rotation = Quaternion.Euler(angleX2, 0, angleZ2);
    }

    void DropRotMov(Transform trans, Vector3 dir, float angleX, float angleY)
    {
        transform.SetPositionAndRotation(trans.position + dir, Quaternion.Euler(angleX, 0, angleY));
    }
    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작할 때 호출하는 인터페이스
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        oriPosition = transform.position;
        oriRotation = transform.eulerAngles; //처음 위치와 회전값 저장
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void OnDrag(PointerEventData eventData) //드래그 중에 호출하는 인터페이스    
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) return;
        if (CameraTrans == null) CameraTrans = Camera.allCameras[0].transform;
        CameraDel = transform.position - CameraTrans.position;

        float cameraDist = CameraDel.magnitude;// 카메라와의 거리 구함
        CameraDel.Normalize();

        Vector3 mousePosition = new (Input.mousePosition.x, Input.mousePosition.y, cameraDist);

        Vector3 objPosition = Camera.allCameras[0].ScreenToWorldPoint(mousePosition);
        objPosition.y = transform.position.y;
        transform.position = objPosition;

        if (Physics.Raycast(transform.position, CameraDel, out RaycastHit hit, 0.9f, dropAble)) // 녹색 상자에서 카메라 방향(z축 앞으로)레이져를 쏴서, 드랍할 수 있는 레이어인지 판단 
        {
            newDragPoint = hit.transform.GetComponent<MeshRenderer>(); // 드랍할 수 있는 오브젝트의 컴포넌트 저장
            if (preDragPoint == newDragPoint) return;
            else
            {
                if (preDragPoint != null) preDragPoint.material.color = ori;//기존 오브젝트의 색을 원본 색으로 되돌림
                ori = newDragPoint.material.color;
                newDragPoint.material.color = Color.yellow;

                Vector3 hitDir = hit.point - hit.transform.position; // 레이저가 닿은 오브젝트의 중심점에서 레이저가 닿은 부분까지 방향을 구함
                if (hitDir.y > hitDir.x && hitDir.y > hitDir.z) transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.z)) DragRot(hitDir.x, 0, 0, -90.0f, 90.0f);
                else if (Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.x)) DragRot(hitDir.z, 90.0f, -90.0f, 0, 0);
                //구한 벡터값으로 접촉 방향을 계산해서 드래그한 오브젝트를 알맞은 회전값으로 돌림
            }
            preDragPoint = hit.transform.GetComponent<MeshRenderer>(); // 새 오브젝트의 값을 기존 오브젝트로 저장
        }
        else if (newDragPoint != null)
        {
            newDragPoint.material.color = ori;
            preDragPoint = null;
            newDragPoint = null;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData) // 드래그가 끝날 때 호출하는 인터페이스
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) return;
        if (Physics.Raycast(transform.position, CameraDel, out RaycastHit hit, 0.9f, dropAble)) // 드래그 오브젝트의 중심점에서 카메라에서 드래그 오브젝트 방향으로 레이져를 쏴서, 드랍할 수 있는 레이어인지 판단 
        {
            Vector3 hitDir = hit.point - hit.transform.position;
            if (hitDir.y > hitDir.x && hitDir.y > hitDir.z) DropRotMov(hit.transform, Vector3.up, 0, 0);

            else if (Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.z))
            {
                if (hitDir.x > 0) DropRotMov(hit.transform, Vector3.right, 0, -90.0f);
                else DropRotMov(hit.transform, Vector3.left, 0, 90.0f);
            }

            else if (Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.x))
            {
                if (hitDir.z > 0) DropRotMov(hit.transform, Vector3.forward, 90.0f, 0);
                else DropRotMov(hit.transform, Vector3.back, -90.0f, 0);
            }
            // 레이저가 닿은 오브젝트의 중심점에서 레이저가 닿은 부분까지 방향을 구함
            // 이후 각 좌표 값을 비교해 알맞은 위치값과 회전값을 구해서 적용함
            newDragPoint.material.color = ori;
        }
        else transform.SetPositionAndRotation(oriPosition, Quaternion.Euler(oriRotation));
         // 드래그를 끝낸 곳이 오브젝트를 놓을 수 있는 곳이 아니라면 처음 드래그한 위치와 회전값으로 돌아감.

        oriPosition = Vector3.zero;
        oriRotation = Vector3.zero;
        CameraTrans = null;
        CameraDel = Vector3.zero;
        preDragPoint = null;
        newDragPoint = null;
        ori = default;
    }
}