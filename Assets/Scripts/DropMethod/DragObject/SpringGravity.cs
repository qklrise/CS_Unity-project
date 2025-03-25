using UnityEngine;

public class SpringGravity : DragAlpha
{
    //BoxCollider boxCol = null;
    //����� �� ���� collider�� �ִ� ������Ʈ���� ������ ���߱� ������
    //isTriger�� Ȱ��ȭ�ϱ� ���� BoxCollider ������Ʈ ������ ����

    protected override void OnDragSet()
    {
        pivotDist = 0.5f;
        base.OnDragSet();
    }
}
