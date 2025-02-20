using System;
using UnityEngine;

public class Doll : AnimProperty
{
    public LayerMask Spring;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & Spring) != 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.LookAt(other.transform.position);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & Spring) != 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.LookAt(other.transform.position);
            }
        }
    }
}
