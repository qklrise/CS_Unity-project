using System;
using UnityEngine;

public class Powdery_Snow : MonoBehaviour
{
    public GameObject player;
    public int a;
    void Start()
    {
    }
    void OnTriggerStay(Collider other)
    {   
        //player.GetComponent<PlayerMove2>().jumpCount = 0;
        Animator anim = player.GetComponent<Animator>();
        anim.SetFloat("Slow", 0.5f);
    }

    void OnTriggerExit(Collider other)
    {
        //player.GetComponent<PlayerMove2>().jumpCount = 2;
        Animator anim = player.GetComponent<Animator>();
        anim.SetFloat("Slow", 1.0f);
    }
}