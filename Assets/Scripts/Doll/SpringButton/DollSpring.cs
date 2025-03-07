using UnityEngine;

public class DollSpring : DollSensor
{
    protected override void Operate()
    {
        foreach (Collider c in list)
        {
            c.GetComponentInParent<Animator>()?.SetTrigger("Using");
            //��ư�� �θ��� spring�� animatior�� Using Ʈ���Ÿ� �� �ִϸ��̼��� ������
        }
    }
}
