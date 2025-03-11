using System.Collections;
using UnityEngine;


public class WhaleMove : MonoBehaviour
{
    public LayerMask playerMask; 
    [SerializeField] float moveSpeed;
    bool isMoving = false; //현재 움직이는지 확인하는 변수
    bool isDown = true; // 현재 내려가 있는지 확인하는 변수
    Transform player;

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & playerMask) != 0 
            && collision.transform.position.y >= transform.position.y + 0.495f)
        {
            // 충돌한 물체가 플레이어 레이어를 가지고 있고, 충돌한 물체가 이 오브젝트 위에서 충돌했다면
            player = collision.transform;
            player.SetParent(transform);
            // 내려갈 때 플레이어와 오브젝트 사이가 계속 벌어져서 함께 움직이게 하기 위해서 자식으로 만듬
            if (!isMoving) StartCoroutine(WhaleMoveCo());
            // 현재 움직이는 상태가 아니라면 움직이는 코루틴 시작
        }
    }

    IEnumerator WhaleMoveCo()
    {
        //거리가 짧아서 코루틴으로 처리하지 않으면 순간이동 해버림
        yield return GameTime.GetWait(0.3f);
        //내려가는 상황에서 자꾸 jump2 애니메이션이 나와서 시작할 때 기다려봤으나 계속 애니메이션이 나옴
        isMoving = true;
        float delta = Time.deltaTime * moveSpeed * 0.13f;
        float moveDist = 1.0f;
        Vector3 MoveDir;
        if (isDown) MoveDir = Vector3.up;
        // 내려가 있는 상태면 움직이는 방향을 위로
        else MoveDir = Vector3.down;
        // 올라가 있는 상태면 움직이는 방향을 아래로
        
        while (!(moveDist < 0 || Mathf.Approximately(moveDist, 0.0f)))
        {
            if (moveDist < delta) delta = moveDist;
            moveDist -= delta;
            transform.Translate(MoveDir * delta);
            yield return null;
        }
        isMoving = false;
        isDown = !isDown;
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((1 << collision.gameObject.layer & playerMask) != 0
            && collision.transform.parent != null)
            collision.transform.SetParent(null);
        // 충돌이 끝나면 설정한 부모 자식 관계를 품
    }
}
