using UnityEngine;
using System.Collections;
public class PlayerControll : AnimProperty
{
    [SerializeField]
    GameObject myCam;
    public GameObject Player;
    public GameObject cameraArm;
    [SerializeField]
    GameObject puzzleCam;
    [SerializeField]
    Rigidbody rig;


    bool onGround = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(ChangeCam());
        }
    }

    IEnumerator ChangeCam()
    {
        if(myCam.activeSelf == true && puzzleCam.activeSelf == false)
        { 
           while (!onGround)
           {
                onGround = Physics.Raycast(transform.position, Vector3.down, 0.1f);
               yield return null;
           }
           if (onGround)
           {
            myAnim.SetTrigger("OnLanding");
            onGround = false;
           }
           
           myCam.SetActive(false);
           puzzleCam.SetActive(true);  
           Player.GetComponent<PlayerMove>().enabled = false;   
           cameraArm.GetComponent<PlayerCam>().enabled = false; 
           Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
           yield return new WaitForSeconds(0.5f);
           
        }

        else if (myCam.activeSelf == false && puzzleCam.activeSelf == true)
        {
            myCam.SetActive(true);
            puzzleCam.SetActive(false);
            Player.GetComponent<PlayerMove>().enabled = true;
            cameraArm.GetComponent<PlayerCam>().enabled = true; 
            Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            yield return new WaitForSeconds(0.5f);
        }
    }
     
    
}
