using UnityEngine;

public class SpringGravity : DropStateGravity
{
    BoxCollider boxCol = null;
    //����� �� ���� collider�� �ִ� ������Ʈ���� ������ ���߱� ������
    //isTriger�� Ȱ��ȭ�ϱ� ���� BoxCollider ������Ʈ ������ ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void ChildFall ()
    {
        boxCol.isTrigger = true;
    }

    protected override void ChildLanding()
    {
        boxCol.isTrigger = false;
    }

    protected override void GetColComponet()
    {
        if (boxCol == null) boxCol = GetComponent<BoxCollider>();
    }
}
