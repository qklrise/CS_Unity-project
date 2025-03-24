using UnityEngine;

public class StopState : StateSet
{
    protected MeshRenderer preDragPoint = null; // 이전 마우스 커서가 있던 바닥의 MeshRenderer 정보
    protected MeshRenderer newDragPoint = null; // 현재 마우스 커서가 있던 바닥의 MeshRenderer 정보
    protected Collider myCol = null;
    protected override void StopSet()
    {
        if (preDragPoint != null) preDragPoint = null;
        if (newDragPoint != null) newDragPoint = null; //변수 초기화
        if (myCol == null) myCol = GetComponent<Collider>();
        if (!myCol.enabled) myCol.enabled = true;
        if (GameManager.isDrag) GameManager.isDrag = false;
        // static 변수 GameManager.isDrag를 변경, drag 상태가 아닌 것으로 설정.
    }

    protected override void Reset()
    {
        if (Input.GetKeyDown(KeyCode.B))
            transform.SetPositionAndRotation(sceanOriPosition, Quaternion.Euler(sceanOriRotation));
        // b 키를 누르면 씬 생성 시의 위치와 회전값으로 돌아감
    }
}
