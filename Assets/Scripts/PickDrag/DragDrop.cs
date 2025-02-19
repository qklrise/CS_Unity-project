using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 oriPosition = Vector3.zero; 
    Vector3 oriRotation = Vector3.zero;
    public LayerMask dropAble; // 드랍할 수 있는 레이어 설정
    Transform CameraTrans = null;
    Vector3 CameraDel = Vector3.zero;
    MeshRenderer preDragPoint = null;
    MeshRenderer newDragPoint = null;

    Color ori = default;

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
        if (CameraTrans == null) CameraTrans = Camera.main.GetComponent<Transform>();
        CameraDel = transform.position - CameraTrans.position;

        float cameraDist = CameraDel.magnitude;// 카메라와의 거리 구함
        CameraDel.Normalize();
        // 마우스 drag로 얻는 좌표는 2종류(기본 x,y 좌표) 3차원 상의 변환을 위해 카메라와 클릭했을 때의 대상의 거리를 계산함.
        // 굳이 이렇게 계산한 이유는 후술함.
        // Camera.main.GetComponent<Transform>().position을 통해 메인카메라의 좌표 값을 얻음

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        // 현재 마우스로 드래그하고 있는 지점의 x,y을 변수에 넣어주고, z값에는 위에서 계산한 z 좌표값을 넣어줌

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objPosition.y = transform.position.y;
        // Camera.main.ScreenToWorldPoint()는 2d 화면 속 벡터를 유티티에서 구현된 3d 벡터로 변경해줌
        // 이때 z좌표만큼 카메라가 찍는 방향으로 이동해서 그 방향에 수직으로 평면을 만듦(디스코드 참조번호 참고 바람)
        // 그 평면 위에 2차원 벡터를 적용해서 3차원 벡터로 변환
        

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
                else if (Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.z))
                {
                    if (hitDir.x > 0) transform.rotation = Quaternion.Euler(0, 0, -90.0f);
                    else transform.rotation = Quaternion.Euler(0, 0, 90.0f);
                }
                else if (Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.x))
                {
                    if (hitDir.z > 0) transform.rotation = Quaternion.Euler(90.0f, 0, 0);
                    else transform.rotation = Quaternion.Euler(-90.0f, 0, 0);
                } //구한 벡터값으로 접촉 방향을 계산해서 드래그한 오브젝트를 알맞은 회전값으로 돌림
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
            if (hitDir.y > hitDir.x && hitDir.y > hitDir.z)
            {
                transform.position = hit.transform.position + Vector3.up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            else if (Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.x) > Mathf.Abs(hitDir.z))
            {
                if (hitDir.x > 0)
                {
                    transform.position = hit.transform.position + Vector3.right;
                    transform.rotation = Quaternion.Euler(0, 0, -90.0f);
                }
                else
                {
                    transform.position = hit.transform.position + Vector3.left;
                    transform.rotation = Quaternion.Euler(0, 0, 90.0f);
                }
            }

            else if (Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.y) && Mathf.Abs(hitDir.z) > Mathf.Abs(hitDir.x))
            {
                if (hitDir.z > 0)
                {
                    transform.position = hit.transform.position + Vector3.forward;
                    transform.rotation = Quaternion.Euler(90.0f, 0, 0);
                }

                else
                {
                    transform.position = hit.transform.position + Vector3.back;
                    transform.rotation = Quaternion.Euler(-90.0f, 0, 0);
                }
            }
            // 레이저가 닿은 오브젝트의 중심점에서 레이저가 닿은 부분까지 방향을 구함
            // 이후 각 좌표 값을 비교해 알맞은 위치값과 회전값을 구해서 적용함
            newDragPoint.material.color = ori;
        }
        else 
        { 
            transform.position = oriPosition;
            transform.rotation = Quaternion.Euler(oriRotation);
        } // 드래그를 끝낸 곳이 오브젝트를 놓을 수 있는 곳이 아니라면 처음 드래그한 위치와 회전값으로 돌아감.

        oriPosition = Vector3.zero;
        oriRotation = Vector3.zero;
        CameraTrans = null;
        CameraDel = Vector3.zero;
        preDragPoint = null;
        newDragPoint = null;
        ori = default;
    }
}
