using UnityEngine;

public class Respawn : MonoBehaviour
{
    public LayerMask playerLayer;
 
    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & playerLayer) != 0)
        {
            collision.transform.position = new Vector3(0, 0, 0);
        }
    }
}
