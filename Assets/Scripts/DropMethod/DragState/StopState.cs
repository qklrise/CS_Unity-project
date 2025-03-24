using UnityEngine;

public class StopState : StateSet
{
    protected MeshRenderer preDragPoint = null; // ���� ���콺 Ŀ���� �ִ� �ٴ��� MeshRenderer ����
    protected MeshRenderer newDragPoint = null; // ���� ���콺 Ŀ���� �ִ� �ٴ��� MeshRenderer ����
    protected Collider myCol = null;
    protected override void StopSet()
    {
        if (preDragPoint != null) preDragPoint = null;
        if (newDragPoint != null) newDragPoint = null; //���� �ʱ�ȭ
        if (myCol == null) myCol = GetComponent<Collider>();
        if (!myCol.enabled) myCol.enabled = true;
        if (GameManager.isDrag) GameManager.isDrag = false;
        // static ���� GameManager.isDrag�� ����, drag ���°� �ƴ� ������ ����.
    }

    protected override void Reset()
    {
        if (Input.GetKeyDown(KeyCode.B))
            transform.SetPositionAndRotation(sceanOriPosition, Quaternion.Euler(sceanOriRotation));
        // b Ű�� ������ �� ���� ���� ��ġ�� ȸ�������� ���ư�
    }
}
