using UnityEngine;

public class DollDrag : UseGravity
{
    
    protected override void RotateMove()
    {
        if (IsRotation)
        {
            if (Input.GetKeyDown(KeyCode.A)) transform.Rotate(0, 90.0f, 0, Space.World);
            else if (Input.GetKeyDown(KeyCode.D)) transform.Rotate(0, -90.0f, 0, Space.World);
            
        }
    }

    protected override void RaySet()
    {
        rayOri = GridMouse + Vector3.up * 0.3f;
        rayDist = floatDist + 1.0f;
    }


    protected override void ChildEndRaySet() //����ؼ� ������ ���� �����Լ�
    {
        if(correctionYpos != 0.5f) correctionYpos = 0.5f;
        // ������ ������(pivot)�� ��ġ�� �ϴ��̹Ƿ�, ������ ������ �Ÿ� ���� ���� ����
    }


}
