using System;
using System.Collections;
using UnityEngine;

public class Spring : AnimProperty
{
    public Rigidbody key;

    private bool canForce = true;
    IEnumerator On()
    {
        canForce = false;
        
        key.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);

        myAnim.SetBool("Using", true);
        yield return new WaitForSeconds(1.0f);
        myAnim.SetBool("Using", false);
        canForce = true;
    }



    public LayerMask Doll;
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
        if (Input.GetKeyDown(KeyCode.E) && canForce)
        {
            if ((1 << other.gameObject.layer & Doll) != 0)
            {
                StartCoroutine(On());
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && canForce)
        {
            if ((1 << other.gameObject.layer & Doll) != 0)
            {
                StartCoroutine(On());
            }
        }
    }


}