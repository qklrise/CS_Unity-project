using System;
using UnityEngine;

public class Powdery_Snow : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    float snowSpeed;
    void Start()
    {
    }
    void OnTriggerStay(Collider other)
    {   
        //player.GetComponent<PlayerMove2>().jumpCount = 0;
        Animator anim = player.GetComponent<Animator>();
        snowSpeed = 0.5f;
        anim.SetFloat("Slow", snowSpeed);
    }

    void OnTriggerExit(Collider other)
    {
        //player.GetComponent<PlayerMove2>().jumpCount = 2;
        Animator anim = player.GetComponent<Animator>();
        snowSpeed = 1.0f;
        anim.SetFloat("Slow", snowSpeed);
    }
}