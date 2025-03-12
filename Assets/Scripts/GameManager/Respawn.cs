using UnityEngine;

public class Respawn : MonoBehaviour
{
    public LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & playerLayer) != 0)
        {
            other.transform.position = new Vector3(0, 3f, 0);
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & playerLayer) != 0)
        {
            collision.transform.position = new Vector3(0, 3f, 0) ;
        }
    }
    */
}
