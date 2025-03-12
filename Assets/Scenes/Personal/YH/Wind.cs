using UnityEngine;

public class Wind : MonoBehaviour
{
    public bool onWind;
    public Rigidbody rig;
    float power;
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody>();   
    }

    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        rig.AddForce(Vector3.forward * power);
    }
}
