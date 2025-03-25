using UnityEngine;

public class SpringGravity : DragAlpha
{
    //BoxCollider boxCol = null;
    //����� �� ���� collider�� �ִ� ������Ʈ���� ������ ���߱� ������
    //isTriger�� Ȱ��ȭ�ϱ� ���� BoxCollider ������Ʈ ������ ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    protected override void StartSet()
    {
        base.StartSet();
        maxRayYAxisVar = 0.5f;
    }
    
}
