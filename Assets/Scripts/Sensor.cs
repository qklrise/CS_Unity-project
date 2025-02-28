using System.Collections;
using UnityEngine;

public class Sensor : AnimProperty
{
    public LayerMask mask;
    public Transform target;
    public BoxCollider collider;

    bool on;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(Searching());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            on = false;
            StartCoroutine(Searching());
        }
        
    }

    IEnumerator Searching()
    {
        while(!on)
        {
            target = null;
            Collider[] list = Physics.OverlapBox(collider.transform.position, collider.size * 0.5f, collider.transform.rotation, mask);
            foreach (Collider c in list)
            {
                target = c.transform;

                c.GetComponentInParent<Animator>()?.SetTrigger("Using");
                on = true;
            }
            yield return null;
        }
    }

}
