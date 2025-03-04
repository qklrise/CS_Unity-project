using UnityEngine;

public class UseGravity : DragState
{
    Rigidbody rb = null; 
    float targetDist = 0.0f; // 드랍할 거리를 저장
    float preYpos = 0.0f; // 이전 y 좌표를 저장
    float newYpos = 0.0f; // 현재 y 좌표를 저장
    float dropDist = 0.0f; // 떨어진 거리를 저장
    float dropTerYpos = 0.0f; // 드랍된 물체의 목표 y 좌표 

    protected override void StartSet()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (!rb.useGravity) rb.useGravity = true;
        if (!rb.isKinematic) rb.isKinematic = true;
        // useGravity로 이동을 제한하면 collider가 있는 오브젝트끼리 충돌했을 때 반동이 있어
        // isKinematic으로 이동을 제한
        GetColComponet();
    }

    protected virtual void GetColComponet() //상속해서 내용을 정할 가상함수
    {

    }

    protected override void EndDragSetRay()
    {
        if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit hit, floatDist + 0.7f, dropAble | stackAble)) 
        {
            if (newDragPoint != null) newDragPoint.material.color = ori;
            //현재 마우스 커서가 있는 바닥의 색을 원래대로 돌림
            rb.isKinematic = false;
            //중력을 활성화하기 위해 isKinematic을 비활성화
            dropTerYpos = hit.transform.position.y + 1.0f;
            //목표 y값을 설정
            targetDist = floatYpos - dropTerYpos;
            // 떨어질 거리 저장
            floatDist = targetDist;
            // 다시 드래그했을 때 띄울 높이 설정
            preYpos = newYpos = floatYpos;
            //현재 높이 정보 저장
            OnTrigger();
        }
        else
        {
            //현재 마우스 커서의 지정한 거리 아래에, 설정한 레이어를 가진 오브젝트가 없다면
            transform.SetPositionAndRotation(dragStartPos, Quaternion.Euler(dragStartRot));
            // 드래그 시작한 위치와 회전 값으로 되돌림
            floatDist = floatYpos - transform.position.y;
            // 다시 드래그했을 때 띄울 높이 설정
            ChangeState(State.Stop);
        }
    }
    protected virtual void OnTrigger() //상속해서 내용을 정할 가상함수
    {

    }
    protected override void EndDragPro()
    {
        newYpos = transform.position.y;
        // 현재 y 좌표 값을 입력
        dropDist = preYpos - newYpos;
        // y 좌표의 차이로 떨어진 거리를 구함
        preYpos = newYpos;
        targetDist -= dropDist;
        //드랍할 목표 거리에서 떨어진 거리를 뺌

        if (Mathf.Approximately(targetDist, 0.0f) || targetDist < 0.0f)
        {
            //드랍할 목표 거리가 0의 근사치거나 0보다 작으면
            OffTrigger();
            rb.isKinematic = true;
            transform.position = new(transform.position.x, dropTerYpos, transform.position.z);
            //중력으로 이동해서 목표한 y좌표에 정확히 도착하지 않기 때문에 y좌표를 직접 설정해줌
            ChangeState(State.Stop);
        }
    }

    protected virtual void OffTrigger() //상속해서 내용을 정할 가상함수
    {

    }
}