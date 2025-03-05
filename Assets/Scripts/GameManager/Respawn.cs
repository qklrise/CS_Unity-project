using UnityEngine;

public class Respawn : MonoBehaviour
{
    LayerMask playerLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            other.transform.position = new Vector3(0, 0, 0);
        }
    }
    
}
