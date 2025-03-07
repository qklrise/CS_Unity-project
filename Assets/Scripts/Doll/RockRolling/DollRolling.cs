using UnityEngine;

public class DollRolling : DollSensor
{
    bool IsRolling = false;
    
    protected override void Operate()
    {
        if(!IsRolling)IsRolling = true; 
    }

    private void FixedUpdate() //�������� ���� ���ϹǷ� FixedUpdate ���
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
