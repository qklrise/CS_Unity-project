using UnityEngine;

public class Respawn : MonoBehaviour
{
    public LayerMask playerLayer;
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
        if ((1<<other.gameObject.layer & playerLayer) != 0)
        {
            other.transform.position = new Vector3(0, 0, 0);
        }
    }
    
}
