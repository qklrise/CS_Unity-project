using UnityEngine;

public class DollRolling : DollSensor
{
    bool IsRolling = false;
    Rigidbody rb = null;
    
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
                if (rb == null) rb = c.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddForce(transform.forward * 1000.0f);
            }
            IsRolling = false;
        }
    }
}
