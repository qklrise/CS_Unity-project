using UnityEngine;

public class SpringGravity : UseGravity
{
    BoxCollider boxCol = null;
    //드랍할 때 서로 collider가 있는 오브젝트끼리 닿으면 멈추기 때문에
    //isTriger를 활성화하기 위해 BoxCollider 컴포넌트 정보를 저장

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void ChildEndRaySet()
    {
        boxCol.isTrigger = true;
    }

    protected override void ChildLanding()
    {
        boxCol.isTrigger = false;
    }

    protected override void GetColComponet()
    {
        if (boxCol == null) boxCol = GetComponent<BoxCollider>();
    }
}
