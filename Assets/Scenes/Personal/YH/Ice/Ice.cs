using UnityEngine;

public class Ice : AnimProperty
{
    public Rigidbody rig;
    public Transform myModel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

void OnTriggerStay(Collider other)
    {
        if(myAnim.speed >= 0)
        {
            rig.AddForce(myModel.forward * 10f);
        }
    }
}
