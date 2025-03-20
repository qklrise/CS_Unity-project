using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Cobweb : MonoBehaviour
{
    public GameObject CobwebHandle;

    public GameObject PlayerCharacter;

    public Transform cameraTransform;

    bool canMove = true;

    RigidbodyConstraints orgRb;


    float maxAngle = 30.0f;
    float add = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Cobweb>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotDir = -cameraTransform.right;
        float delta = 100.0f * Time.deltaTime;

        if (Input.GetKey(KeyCode.W) && canMove)
        {
            Swing(rotDir);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            StartCoroutine(resetRot());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Out();
        }
    }

    void Swing(Vector3 rotDir)
    {
        if (maxAngle > 0f)
        {
            float delta = Time.deltaTime * 90.0f;
            if (delta > maxAngle)
            {
                StartCoroutine(resetRot());
                maxAngle = 30.0f + add;

            }
            maxAngle -= delta;
            CobwebHandle.transform.Rotate(rotDir * delta);
        }
    }

    IEnumerator resetRot()
    {
        canMove = false;

        while (CobwebHandle.transform.rotation != Quaternion.identity)
        {
            CobwebHandle.transform.rotation = Quaternion.Lerp(CobwebHandle.transform.rotation, Quaternion.identity, 5.0f * Time.deltaTime);
            yield return null;
        }
        add += 10.0f;

        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        In();
    }
    private void OnTriggerExit(Collider other)
    {
        Out();
    }

    void In()
    {
            GetComponent<Cobweb>().enabled = true;

            PlayerCharacter.GetComponent<PlayerMove2>().enabled = false;
            PlayerCharacter.GetComponentInChildren<Interaction>().enabled = false;

            Rigidbody rb = PlayerCharacter.GetComponent<Rigidbody>();
            orgRb = rb.constraints;

            rb.constraints = RigidbodyConstraints.FreezeAll;
            PlayerCharacter.GetComponent<Rigidbody>().constraints = orgRb; // ���ӵ��� �ʱ�ȭ

            PlayerCharacter.GetComponent<Rigidbody>().useGravity = false;

            PlayerCharacter.transform.SetParent(CobwebHandle.transform);
    }

    void Out()
    {
        GetComponent<Cobweb>().enabled = false;
        PlayerCharacter.GetComponent<PlayerMove2>().enabled = true;
        PlayerCharacter.GetComponentInChildren<Interaction>().enabled = true;
        PlayerCharacter.GetComponent<Rigidbody>().useGravity = true;

        PlayerCharacter.transform.SetParent(PlayerCharacter.transform);
    }

}

// �ִ� ���� ������ �ɾ�ΰ� �� ���ѿ� ������ ������ �ִ� ������ ���ݾ� �ø��� ������ ���� ����?
// �� ���� �ִ� ������ �����ϸ� �� �� ���� ��Ȱ��ȭ, ����ġ�� ���ƿ���
