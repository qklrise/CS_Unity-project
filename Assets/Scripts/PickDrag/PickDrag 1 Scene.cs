using UnityEngine;

// !! 최중요 사항: 레이저를 활용하는 방법이기 때문에 이동할 대상의 Collider가 활성화돼야만 3방법 모두 작동할 수 있음 !!

// 수업에서 배운 내용을 바탕으로 구현해본 drag 이동
// 함수나 인터페이스를 쓰는 방법에 비해 코드 길이가 김
// 유니티에서 마우스 드래그를 통해 얻는 위치 변화(벡터) 값은 3차원 값이지만,
// 이 방법은 2차원으로 표현된 모니터 화면에서의 마우스 위치 좌표로 위치 변화(벡터) 값을 구해 이동해서 오차가 존재함.(디스코드 참조번호 참고)
// 대신 이동할 대상에는 레이어만 지정해주고, 스크립트는 아무 오브젝트에 넣어도 상관이 없음.
// (해당 씬에는 플레이어 오브젝트와 메인 카메라에 스크립트를 넣어줌)

public class PickDrag : MonoBehaviour
{
    public LayerMask pickAbleMask;

    Transform target = null; // drag를 통해 이동시킬 대상의 정보를 저장할 변수 
    public float DragMoveSpeed = 5.0f;

    Vector3 ReferenceMousePosition = Vector3.zero; // 이동을 시작하는 지점을 저장하는 변수

    void PickObject()
    {
        if(Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭을 할 때, 즉 드래그 시작 시점
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, pickAbleMask))
            {
                target = hit.transform; 
                ReferenceMousePosition = Input.mousePosition; //클릭을 한 좌표를 기준점으로 저장
                target.Translate(Vector3.back * 1.0f,Space.World); // 대상이 좌클릭으로 선택됐다는 것을 표시하기 위해 z 좌표를 변경함.
            }
        }

        else if ( target != null && Input.GetMouseButton(0)) 
        {
            Vector3 moveDir2D = Input.mousePosition - ReferenceMousePosition; // 모니터 화면(2D)에서 현재 마우스 좌표 값과 기준점의 차이를 통해 이동할 벡터값을 구함
            ReferenceMousePosition = Input.mousePosition; //기준값을 현재 마우스 좌표값으로 갱신

            float moveDist = moveDir2D.magnitude; //구한 벡터값으로 대상을 현재 위치에서 계산한 벡터값만큼 이동시킴
            moveDir2D.Normalize();

            if (!Mathf.Approximately(moveDist, 0.0f))
             {
                float delta = DragMoveSpeed * Time.deltaTime;
                if (delta > moveDist) delta = moveDist;
                target.Translate(moveDir2D * delta, Space.World);
                moveDist -= delta;
             }
        }
        
        else if(target != null && !Input.GetMouseButton(0))
        {
            target.Translate(Vector3.forward * 1.0f, Space.World); //drag가 끝났기에 대상의 z 좌표를 원래 위치로 되돌림.
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PickObject();
    }
}
