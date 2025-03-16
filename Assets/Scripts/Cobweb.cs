using UnityEngine;

public class Cobweb : MonoBehaviour
{
    public GameObject CobwebHandle;

    GameObject default_CobwebHandle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Cobweb>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float targetDist = Time.deltaTime * 1.0f;

        if (CobwebHandle.transform.rotation != default_CobwebHandle.transform.rotation)
        {
            CobwebHandle.transform.rotation = Quaternion.Lerp(CobwebHandle.transform.rotation, default_CobwebHandle.transform.rotation, targetDist);
        }
        */

        if (Input.GetKey(KeyCode.W))
        {
            CobwebHandle.transform.Rotate(transform.forward * Time.deltaTime * 10.0f);
            //CobwebHandle.transform.rotation;
        }
        if (Input.GetKey(KeyCode.S))
        {

        }
        if (Input.GetKey(KeyCode.A))
        {

        }
        if (Input.GetKey(KeyCode.D))
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {

        GetComponent<Cobweb>().enabled = true;

        other.GetComponent<PlayerMove2>().enabled = false;
        other.GetComponentInChildren<Interaction>().enabled = false;
        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        other.GetComponent<Rigidbody>().useGravity = false;
        
        other.transform.SetParent(CobwebHandle.transform);
        
    }
    private void OnTriggerExit(Collider other)
    {

        GetComponent<Cobweb>().enabled = false;
        other.GetComponent<PlayerMove2>().enabled = true;
        other.GetComponentInChildren<Interaction>().enabled = true;
        other.GetComponent<Rigidbody>().useGravity = true;

        other.transform.SetParent(null);
        
    }
}
