using UnityEngine;
using System.Collections;
public class PlayerControll : AnimProperty
{
    public GameObject Player;
    public GameObject cameraArm;
    [SerializeField]
    GameObject puzzleCam;
    Rigidbody rig;
    [SerializeField]
    Animator playerAnim;


    bool onGround = false;
    void Start()
    {
        rig = Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameManager.ChangeGameMode();
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

           cameraArm.SetActive(false);
           puzzleCam.SetActive(true);  
           Player.GetComponent<PlayerMove2>().enabled = false;   
           rig.constraints = RigidbodyConstraints.FreezeAll;
           rig.linearVelocity = Vector3.zero;
           playerAnim.SetFloat("Speed", 0.0f);
           yield return GameTime.GetWait(0.5f);
           
        }
        
        

        else if (GameManager.isPuzzle == false)
        {
            cameraArm.SetActive(true);
            puzzleCam.SetActive(false);
            Player.GetComponent<PlayerMove2>().enabled = true;
            rig.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            GameManager.isPuzzle = false;
            yield return GameTime.GetWait(0.5f);
        }
    }
     
    
}
