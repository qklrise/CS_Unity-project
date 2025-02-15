using UnityEngine;
using UnityEngine.EventSystems;

// !! 최중요 사항: 레이저를 활용하는 방법이기 때문에 이동할 대상의 Collider가 활성화돼야만 3방법 모두 작동할 수 있음 !!

// 유니티에 내장된 인터페이스를 이용한 방법
// using UnityEngine.EventSystems을 포함해야 함
// 2번 방법과 인터페이스와 함수의 차이를 제외하면 코드 내용은 동일함.
// 마우스 입력을 받는 것이 주 사용법인 2번 방법에 비해, 3번은 다양한 입력 장치를 받을 수 있다함(게임패드 등)
// 대상에 직접 스크립트를 추가해줘야 함
// 카메라 각도에 따라서 대상의 z 좌표 값도 변경 가능함.
// 3번은 추가로 카메라에 Physics Raycaster 컴포넌트 추가하고, Hierarchy에 EventSyetem 오브젝트 추가해야 작동함(디스코드 참고 자료 참조바람)
// 2번의 OnMouseDown()함수는 클릭에 호출되서 클릭하자마자 반응하지만, 3번의 OnBeginDrag은 단순 클릭하면 호출이 안 되고 드래그가 시작될 때 호출.
// 그래서 2번은 더블클릭에 반응하고 3번은 반응하지 않음.

public class PickDrag3Scean: MonoBehaviour,IBeginDragHandler, IDragHandler,IEndDragHandler 
{
    Vector3 oriPosition = Vector3.zero;
    Vector3 fromCameraVector = Vector3.zero;

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작할 때 호출하는 인터페이스
    {
        transform.Translate(Vector3.back * 1.0f, Space.World); //선택한 대상을 구분하기 쉽기 위해 z축으로 옮김
        if (oriPosition == Vector3.zero) oriPosition = transform.position;
    }
    public void OnDrag(PointerEventData eventData) //드래그 중에 호출하는 인터페이스
    {
        fromCameraVector = Camera.main.GetComponent<Transform>().position - oriPosition;
        float cameraDist = fromCameraVector.magnitude;

        // 마우스 drag로 얻는 좌표는 2종류(기본 x,y 좌표) 3차원 상의 변환을 위해 카메라와 클릭했을 때의 대상의 거리를 계산함.
        // 굳이 이렇게 계산한 이유는 후술함.
        // Camera.main.GetComponent<Transform>().position을 통해 메인카메라의 좌표 값을 얻음

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDist);
        // 현재 마우스로 드래그하고 있는 지점의 x,y을 변수에 넣어주고, z값에는 위에서 계산한 z 좌표값을 넣어줌

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // Camera.main.ScreenToWorldPoint()는 2d 화면 속 벡터를 유티티에서 구현된 3d 벡터로 변경해줌
        // 이때 z좌표만큼 카메라가 찍는 방향으로 이동해서 그 방향에 수직으로 평면을 만듦(디스코드 참조번호 참고 바람)
        // 그 평면 위에 2차원 벡터를 적용해서 3차원 벡터로 변환
        // 지금 올리는 씬들은 모두 카메라 회전 값이 (0,0,0)으로 카메라가 z축 방향으로 찍고 있어서
        // z축에 수직인 평면이 만들어져서 드래그하는 동안 즉 드래그 하는 동안에는 z 좌표가 고정된 상태임.

        transform.position = objPosition;
    }

    public void OnEndDrag(PointerEventData eventData) // 드래그가 끝날 때 호출하는 인터페이스
    {
        //if() oriPosition = Vector3.zero; //특정 조건에서 기준점을 초기화 해야함.
        //현 시점에서 드래그할 때마다 기준점을 바꾸면 오차가 생겨, 오브젝트가 원하지 않는 위치로 이동함.(주로 z좌표가 증가하여 벽을 통과함)

        transform.Translate(Vector3.forward * 1.0f, Space.World); // drag가 끝나 증가한 z좌표를 되돌림.
    }
}
