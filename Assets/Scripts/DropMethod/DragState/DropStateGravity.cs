using UnityEngine;

public class DropStateGravity : DragRotate
{
    float targetDist = 0.0f; // ����� �Ÿ��� ����
    float preYpos = 0.0f; // ���� y ��ǥ�� ����
    float newYpos = 0.0f; // ���� y ��ǥ�� ����
    float dropDist = 0.0f; // ������ �Ÿ��� ����
    float dropTerYpos = 0.0f; // ����� ��ü�� ��ǥ y ��ǥ 
    protected float correctionYpos = 1.0f; // �������� ���� ������Ʈ�� ������(pivot)���� �巡�� ���� ������Ʈ�� ������(pivot)������ �Ÿ�

    protected override void StartSet()
    {
        if (!rb.useGravity) rb.useGravity = true;
        GetColComponet();
    }

    protected virtual void GetColComponet() //����ؼ� ������ ���� �����Լ�
    {

    }

    protected override void EndDragSet()
    {
        if (IsRotation) IsRotation = false;
        // ȸ�� ���¸� �� Ǯ���ٸ� �巡�װ� ���� �� ȸ�� ���°� �ƴ� ������ ����
        if (camMove != null && !camMove.enabled) camMove.enabled = true;
        // ȸ�� ���¸� �� Ǯ���ٸ� �巡�װ� ���� �� puzzleCam�� ������ ������Ʈ�� Ȱ��ȭ

        if (canDrop)
        {
            ChildFall();
            if (newDragPoint != null) newDragPoint.material.color = ori;
            //���� ���콺 Ŀ���� �ִ� �ٴ��� ���� ������� ����
            if(rb.isKinematic) rb.isKinematic = false;
            //�߷��� Ȱ��ȭ�ϱ� ���� isKinematic�� ��Ȱ��ȭ
            dropTerYpos = rayHitTranY + correctionYpos;
            //��ǥ y���� ����
            targetDist = floatYpos - dropTerYpos;
            // ������ �Ÿ� ����
            floatDist = targetDist;
            // �ٽ� �巡������ �� ��� ���� ����
            preYpos = newYpos = floatYpos;
            //���� ���� ���� ����
        }
        else
        {
            if(newDragPoint != null) newDragPoint.material.color = ori;
            //���� ���콺 Ŀ���� ������ �Ÿ� �Ʒ���, ������ ���̾ ���� ������Ʈ�� ���ٸ�
            transform.SetPositionAndRotation(dragStartPos, Quaternion.Euler(dragStartRot));
            // �巡�� ������ ��ġ�� ȸ�� ������ �ǵ���
            floatDist = floatYpos - transform.position.y;
            // �ٽ� �巡������ �� ��� ���� ����
            ChangeState(State.Stop);
        }
    }
    
    protected virtual void ChildFall() //����ؼ� ������ ���� �����Լ�
    {

    }
    protected override void EndDragPro()
    {
        newYpos = transform.position.y;
        // ���� y ��ǥ ���� �Է�
        dropDist = preYpos - newYpos;
        // y ��ǥ�� ���̷� ������ �Ÿ��� ����
        preYpos = newYpos;
        targetDist -= dropDist;
        //����� ��ǥ �Ÿ����� ������ �Ÿ��� ��

        if (Mathf.Approximately(targetDist, 0.0f) || targetDist < 0.0f)
        {
            //����� ��ǥ �Ÿ��� 0�� �ٻ�ġ�ų� 0���� ������
            ChildLanding();
            rb.isKinematic = true;
            transform.position = new(transform.position.x, dropTerYpos, transform.position.z);
            //�߷����� �̵��ؼ� ��ǥ�� y��ǥ�� ��Ȯ�� �������� �ʱ� ������ y��ǥ�� ���� ��������
            ChangeState(State.Stop);
        }
    }

    protected virtual void ChildLanding() //����ؼ� ������ ���� �����Լ�
    {

    }
}