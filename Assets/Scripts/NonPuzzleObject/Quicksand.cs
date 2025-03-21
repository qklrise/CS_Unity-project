using System.Collections;
using UnityEngine;

public class Quicksand : MonoBehaviour
{
    [SerializeField] float sandDepth = 0.65f;
    [SerializeField] float fallSpeed = 1.0f;
    public LayerMask playerMask;
    Rigidbody playerRb = null;
    Collider myCol = null;
    Transform playerTrans = null;
    PlayerMove2 move2 = null;
    Coroutine QuicksandHallCo = null;

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & playerMask) != 0)
        {
            if(playerRb == null) playerRb = collision.gameObject.GetComponent<Rigidbody>();
            playerRb.useGravity = false;
            playerRb.linearVelocity = Vector3.zero;

            if(myCol == null) myCol = GetComponent<Collider>();
            myCol.isTrigger = true;

            if(playerTrans == null) playerTrans = collision.gameObject.transform;
             if(QuicksandHallCo == null) QuicksandHallCo = StartCoroutine(QuicksandHall(playerTrans));
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (move2 == null) move2 = other.gameObject.GetComponent<PlayerMove2>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (QuicksandHallCo != null)
            {
                StopCoroutine(QuicksandHallCo);
                QuicksandHallCo = null;
            }
            if (move2.onGround) move2.onGround = false;
            if (!playerRb.useGravity) playerRb.useGravity = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & playerMask) != 0)
        {
            if (!playerRb.useGravity) playerRb.useGravity = true;
            if (myCol.isTrigger) myCol.isTrigger = false;
        }
    }

    IEnumerator QuicksandHall(Transform trans)
    { 
        Vector3 moveDir = this.transform.position - trans.position + Vector3.down * sandDepth;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();
        float delta = Time.deltaTime * fallSpeed;

        while (moveDist > 0) 
        {
            if(moveDist < delta) delta = moveDist;
            moveDist -= delta;
            trans.Translate(moveDir * delta); 
            yield return null;
        }
    }
}
