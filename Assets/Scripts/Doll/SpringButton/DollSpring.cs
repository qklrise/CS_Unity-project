using UnityEngine;

public class DollSpring : DollSensor
{
    protected override void Operate()
    {
        foreach (Collider c in list)
        {
            c.GetComponentInParent<Animator>()?.SetTrigger("Using");
            //버튼의 부모인 spring의 animatior의 Using 트리거를 켜 애니메이션을 시작함
        }
    }
}
