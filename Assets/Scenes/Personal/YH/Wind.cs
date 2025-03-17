using UnityEngine;

public class Wind : MonoBehaviour
{
    public bool onWind;
    public Rigidbody rig;
    public float power;
    public Vector3 windDirection;
    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        rig.AddForce(windDirection *power);
    }
}
