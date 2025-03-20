using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Cobweb2 : MonoBehaviour
{
    public GameObject CobwebHandle;
    public GameObject Cobweb;
    public GameObject Cobweb1;

    public LayerMask PlayerMask;

    public GameObject PlayerCharacter;

    public Transform cameraTransform;

    bool canMove = true;

    RigidbodyConstraints orgRb;
    RigidbodyConstraints CobwebOrgRb;

    float maxAngle = 30.0f;
    float add = 0.0f;

    Vector3 defaultCameraPos;
    Quaternion defaultCameraRot;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Cobweb2>().enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotDir = -cameraTransform.right;

        float rotY = cameraTransform.rotation.eulerAngles.y;
        Vector3 forward = Quaternion.Euler(0, rotY, 0) * Vector3.forward;

        CobwebHandle.transform.forward = forward;

        float delta = 100.0f * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            Swing(rotDir);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //StartCoroutine(resetRot());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Out();
        }
    }

    void Swing(Vector3 rotDir)
    {
        Cobweb.GetComponent<Rigidbody>().AddForce(rotDir * 10.0f, ForceMode.Impulse);
    }

    IEnumerator resetRot()
    {
        canMove = false;

        Cobweb.GetComponent<Rigidbody>().isKinematic = true;
        while (Cobweb.transform.rotation != Quaternion.identity)
        {
            Cobweb.transform.rotation = Quaternion.Lerp(Cobweb.transform.rotation, Quaternion.identity, 5.0f * Time.deltaTime);
            yield return null;
        }
        add += 10.0f;

        Cobweb.GetComponent<Rigidbody>().isKinematic = false;
        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & PlayerMask) != 0) In();
    }
    private void OnTriggerExit(Collider other)
    {
        Out();
    }

    void In()
    {
        GetComponent<Cobweb2>().enabled = true;
        

        PlayerCharacter.GetComponent<PlayerMove2>().enabled = false;
        PlayerCharacter.GetComponentInChildren<Interaction>().enabled = false;
     
        //---------------------------------------------------------------------------
        Rigidbody rb = PlayerCharacter.GetComponent<Rigidbody>();
        orgRb = rb.constraints;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        PlayerCharacter.GetComponent<Rigidbody>().constraints = orgRb;
        // 가속도를 초기화
        //---------------------------------------------------------------------------

        PlayerCharacter.GetComponent<Rigidbody>().useGravity = false;

        PlayerCharacter.transform.SetParent(Cobweb1.transform);
        cameraTransform.SetParent(null);
    }

    void Out()
    {
        GetComponent<Cobweb2>().enabled = false;
        PlayerCharacter.GetComponent<PlayerMove2>().enabled = true;
        PlayerCharacter.GetComponentInChildren<Interaction>().enabled = true;
        PlayerCharacter.GetComponent<Rigidbody>().useGravity = true;

        PlayerCharacter.transform.SetParent(null);
    }

}

// 최대 각도 제한을 걸어두고 그 제한에 도달할 때마다 최대 각도를 조금씩 늘리는 식으로 가속 구현?
// 현 상태 최대 각도에 도달하면 그 때 조작 비활성화, 원위치로 돌아오기
