using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Doll : AnimProperty
{
    public LayerMask Button;
    public BoxCollider collider;
    public Transform target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] list = Physics.OverlapBox(collider.center, collider.size * 0.5f, collider.transform.rotation);
            foreach (Collider c in list)
            {
                if ((1 << c.gameObject.layer * Button) != 0)
                {
                    c.GetComponentInParent<Animator>()?.SetTrigger("Using");
                }
            }
        }
        */

    }

    IEnumerator Searching()
    {
        while (true)
        {
            target = null;
            Collider[] list = Physics.OverlapBox(collider.transform.position, collider.size * 0.5f, collider.transform.rotation);
            foreach (Collider c in list)
            {
                target = c.transform;
                c.GetComponent<Animator>()?.SetTrigger("Using");
            }
            yield return null;
        }
    }
}
