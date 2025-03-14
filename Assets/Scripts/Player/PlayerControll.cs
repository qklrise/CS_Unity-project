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
    [SerializeField]
    Animator playerAnim;


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
        
        if(GameManager.isPuzzle == true)
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
           Player.GetComponent<PlayerMove2>().enabled = false;   
           cameraArm.GetComponent<PlayerCam>().enabled = false; 
           Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
           rig.linearVelocity = Vector3.zero;
           playerAnim.SetFloat("Speed", 0.0f);
           yield return new WaitForSeconds(0.5f);
           
        }
        
        

        else if (GameManager.isPuzzle == false)
        {
            myCam.SetActive(true);
            puzzleCam.SetActive(false);
            Player.GetComponent<PlayerMove2>().enabled = true;
            cameraArm.GetComponent<PlayerCam>().enabled = true; 
            Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            GameManager.isPuzzle = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
     
    
}
