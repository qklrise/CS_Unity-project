using UnityEngine;

public class Respawn : MonoBehaviour
{
    public LayerMask playerLayer;
 
    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & playerLayer) != 0)
        {
            other.transform.position = new Vector3(0, 0, 0);
        }
    }
}
