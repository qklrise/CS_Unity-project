using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public LayerMask Key;
    public GameObject KeyObj;

    IEnumerator Open()
    {
        yield return new WaitForSeconds(4.3f);
        Destroy(KeyObj);
        Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if ((1 << other.gameObject.layer & Key) != 0)
            {
                StartCoroutine(Open());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if ((1 << other.gameObject.layer & Key) != 0)
            {
                StartCoroutine(Open());
            }
        }
    }
}
