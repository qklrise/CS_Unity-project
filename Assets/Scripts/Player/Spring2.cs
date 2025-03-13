using UnityEngine;

public class Spring2 : MonoBehaviour
{
    public Transform myPannel;
    public LayerMask pushLayer;

    public void OnPush() // 애니메이션 이벤트로 호출하는 함수
    {
        Collider[] list = Physics.OverlapBox(transform.position + transform.up * 1.5f, new Vector3(0.5f, 0.5f, 0.5f), transform.rotation, pushLayer); // 함수가 실행될 때 위에 놓인 것들을 찾음
        // 판넬 방향으로 한칸 이동한 정육면체 모양의 감지 모양을 만듦
        foreach (Collider col in list)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // 힘을 받아 움직이도록 isKinematic을 끔
                rb.AddForce(transform.up * 900.0f);  // 찾아진 오브젝트에 릿지드 바디가 있으면 해당 오브젝트를 밈
            } 
        }
    }
}