using UnityEngine;

public class DollRolling : DollSensor
{
    bool IsRolling = false;
    
    protected override void Operate()
    {
        if(!IsRolling)IsRolling = true; 
    }

    private void FixedUpdate() //물리적인 힘을 가하므로 FixedUpdate 사용
    {
        if (IsRolling)
        {
            foreach (Collider c in list)
            {
                c.GetComponent<Rigidbody>().AddForce(transform.forward * 1000.0f);
            }
            IsRolling = false;
        }
    }
}
