using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Tilemaps;
public class TimeTile : MonoBehaviour
{
    //public GameObject Tile;
    public LayerMask playerMask; 
    bool isOntile = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOntile)
        {
            isOntile = true;
            StartCoroutine(OnTile());
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & playerMask) != 0 
            && collision.transform.position.y >= transform.position.y + 0.5f)
            {
                StartCoroutine(OffTile());
            }
    }
    IEnumerator OffTile()
    {
        yield return new WaitForSeconds(3.0f);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        isOntile = false;
    }
    IEnumerator OnTile()
    {
        yield return new WaitForSeconds(3.0f);
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<Collider>().enabled = true;
    }
}


