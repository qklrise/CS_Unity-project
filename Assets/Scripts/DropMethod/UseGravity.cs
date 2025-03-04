using UnityEngine;

public class UseGravity : DragState
{
    Rigidbody rb = null; 
    float targetDist = 0.0f; // ����� �Ÿ��� ����
    float preYpos = 0.0f; // ���� y ��ǥ�� ����
    float newYpos = 0.0f; // ���� y ��ǥ�� ����
    float dropDist = 0.0f; // ������ �Ÿ��� ����
    float dropTerYpos = 0.0f; // ����� ��ü�� ��ǥ y ��ǥ 

    protected override void StartSet()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (!rb.useGravity) rb.useGravity = true;
        if (!rb.isKinematic) rb.isKinematic = true;
        // useGravity�� �̵��� �����ϸ� collider�� �ִ� ������Ʈ���� �浹���� �� �ݵ��� �־�
        // isKinematic���� �̵��� ����
        GetColComponet();
    }

    protected virtual void GetColComponet() //����ؼ� ������ ���� �����Լ�
    {

    }

    protected override void EndDragSetRay()
    {
        if (Physics.Raycast(GridMouse, Vector3.down, out RaycastHit hit, floatDist + 0.7f, dropAble | stackAble)) 
        {
            if (newDragPoint != null) newDragPoint.material.color = ori;
            //���� ���콺 Ŀ���� �ִ� �ٴ��� ���� ������� ����
            rb.isKinematic = false;
            //�߷��� Ȱ��ȭ�ϱ� ���� isKinematic�� ��Ȱ��ȭ
            dropTerYpos = hit.transform.position.y + 1.0f;
            //��ǥ y���� ����
            targetDist = floatYpos - dropTerYpos;
            // ������ �Ÿ� ����
            floatDist = targetDist;
            // �ٽ� �巡������ �� ��� ���� ����
            preYpos = newYpos = floatYpos;
            //���� ���� ���� ����
            OnTrigger();
        }
        else
        {
            //���� ���콺 Ŀ���� ������ �Ÿ� �Ʒ���, ������ ���̾ ���� ������Ʈ�� ���ٸ�
            transform.SetPositionAndRotation(dragStartPos, Quaternion.Euler(dragStartRot));
            // �巡�� ������ ��ġ�� ȸ�� ������ �ǵ���
            floatDist = floatYpos - transform.position.y;
            // �ٽ� �巡������ �� ��� ���� ����
            ChangeState(State.Stop);
        }
    }
    protected virtual void OnTrigger() //����ؼ� ������ ���� �����Լ�
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
            OffTrigger();
            rb.isKinematic = true;
            transform.position = new(transform.position.x, dropTerYpos, transform.position.z);
            //�߷����� �̵��ؼ� ��ǥ�� y��ǥ�� ��Ȯ�� �������� �ʱ� ������ y��ǥ�� ���� ��������
            ChangeState(State.Stop);
        }
    }

    protected virtual void OffTrigger() //����ؼ� ������ ���� �����Լ�
    {

    }
}