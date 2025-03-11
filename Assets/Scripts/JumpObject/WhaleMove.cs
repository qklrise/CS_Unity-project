using System.Collections;
using UnityEngine;


public class WhaleMove : MonoBehaviour
{
    public LayerMask playerMask; 
    [SerializeField] float moveSpeed;
    bool isMoving = false; //���� �����̴��� Ȯ���ϴ� ����
    bool isDown = true; // ���� ������ �ִ��� Ȯ���ϴ� ����
    Transform player;

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & playerMask) != 0 
            && collision.transform.position.y >= transform.position.y + 0.495f)
        {
            // �浹�� ��ü�� �÷��̾� ���̾ ������ �ְ�, �浹�� ��ü�� �� ������Ʈ ������ �浹�ߴٸ�
            player = collision.transform;
            player.SetParent(transform);
            // ������ �� �÷��̾�� ������Ʈ ���̰� ��� �������� �Բ� �����̰� �ϱ� ���ؼ� �ڽ����� ����
            if (!isMoving) StartCoroutine(WhaleMoveCo());
            // ���� �����̴� ���°� �ƴ϶�� �����̴� �ڷ�ƾ ����
        }
    }

    IEnumerator WhaleMoveCo()
    {
        //�Ÿ��� ª�Ƽ� �ڷ�ƾ���� ó������ ������ �����̵� �ع���
        yield return GameTime.GetWait(0.3f);
        //�������� ��Ȳ���� �ڲ� jump2 �ִϸ��̼��� ���ͼ� ������ �� ��ٷ������� ��� �ִϸ��̼��� ����
        isMoving = true;
        float delta = Time.deltaTime * moveSpeed * 0.13f;
        float moveDist = 1.0f;
        Vector3 MoveDir;
        if (isDown) MoveDir = Vector3.up;
        // ������ �ִ� ���¸� �����̴� ������ ����
        else MoveDir = Vector3.down;
        // �ö� �ִ� ���¸� �����̴� ������ �Ʒ���
        
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
        // �浹�� ������ ������ �θ� �ڽ� ���踦 ǰ
    }
}
