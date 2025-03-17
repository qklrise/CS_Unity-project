using UnityEngine;

public class DragRotate : DragState
{
    protected bool IsRotation = false; // 현재 회전 상태 여부 확인
    protected PuzzleCamMove camMove = null; // PuzzleCamMove 활성화 여부 결정

    protected override void EnterRotate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            IsRotation = !IsRotation;
            if (camMove == null) camMove = puzzleCameraArm.GetComponent<PuzzleCamMove>();
            if (camMove.enabled) camMove.enabled = false;
            else camMove.enabled = true;
            //회전 상태와 캠이동 컴포넌트를 상태를 변경
        }
    }
    protected override void RotateMove()
    {
        if (IsRotation)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                RotateDir(puzzleCameraArm.transform.right);
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                RotateDir(-puzzleCameraArm.transform.right);
            }

            else if (Input.GetKeyDown(KeyCode.A))
            {
                RotateDir(puzzleCameraArm.transform.up);
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateDir(-puzzleCameraArm.transform.up);
            }

            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateDir(puzzleCameraArm.transform.forward);
            }

            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateDir(-puzzleCameraArm.transform.forward);
            }

        }
        // 회전상태일 때 wasd로 회전
    }

    void RotateDir(Vector3 dir)
    {
        Vector3 rotKeyDown = dir;
        RotateAngleSet(ref rotKeyDown.x, ref rotKeyDown.y, ref rotKeyDown.z);
        transform.Rotate(rotKeyDown * 90.0f, Space.World);
    }

    void RotateAngleSet(ref float x, ref float y, ref float z)
    {
        if (Mathf.Abs(x) >= Mathf.Abs(y) && Mathf.Abs(x) >= Mathf.Abs(z))
        {
            CompareAxis(ref x, ref y, ref z);
        }

        else if (Mathf.Abs(y) > Mathf.Abs(x) && Mathf.Abs(y) >= Mathf.Abs(z))
        {
            CompareAxis(ref y, ref x, ref z);
        }

        else if (Mathf.Abs(z) > Mathf.Abs(x) && Mathf.Abs(z) > Mathf.Abs(y))
        {
            CompareAxis(ref z, ref x, ref y);
        }

    }

    void CompareAxis(ref float a, ref float b, ref float c)
    {
        if (a < 0) a = -1;
        else a = 1;
        b = 0f;
        c = 0f;
    }

}
