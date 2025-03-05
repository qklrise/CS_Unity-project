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


    protected override void ChildEndRaySet() //상속해서 내용을 정할 가상함수
    {
        if(correctionYpos != 0.5f) correctionYpos = 0.5f;
        // 인형은 기준점(pivot)의 위치가 하단이므로, 기준점 사이의 거리 변수 값을 변경
    }


}
