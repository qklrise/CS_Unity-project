using UnityEngine;

public class DollDrag : UseGravity
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void RotateMove()
    {
        if (IsRotation)
        {
            if (Input.GetKeyDown(KeyCode.A)) transform.Rotate(0, 90.0f, 0, Space.World);
            else if (Input.GetKeyDown(KeyCode.D)) transform.Rotate(0, -90.0f, 0, Space.World);
            
        }
    }
}
