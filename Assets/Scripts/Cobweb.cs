using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Cobweb : MonoBehaviour
{
    public GameObject CobwebHandle;

    public LayerMask Player;

    public GameObject PlayerCharacter;
    public GameObject PlayerCharacterParent;

    public Transform cameraTransform;

    bool reset = true;

    float maxAngle = 30.0f;


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

        if (Input.GetKey(KeyCode.W))
        {
            StopAllCoroutines();
            StartCoroutine(Rotating(rotDir));
        }
        if (maxAngle >= 90) maxAngle = 90;
    }

    IEnumerator Rotating(Vector3 rotDir)
    {
        while (maxAngle > 0f)
        {
            float delta = Time.deltaTime * 90.0f;
            if (delta > maxAngle) delta = maxAngle;
            maxAngle -= delta;
            CobwebHandle.transform.Rotate(rotDir * delta);
            yield return null;
        }

        StartCoroutine(resetRot());
        maxAngle += 10.0f;
    }

    IEnumerator resetRot()
    {
        while (CobwebHandle.transform.rotation != Quaternion.identity)
        {
            CobwebHandle.transform.rotation = Quaternion.Lerp(CobwebHandle.transform.rotation, Quaternion.identity, Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & Player) != 0)
        {
            GetComponent<Cobweb>().enabled = true;

            other.GetComponent<PlayerMove2>().enabled = false;
            other.GetComponentInChildren<Interaction>().enabled = false;

            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // 가속도를 초기화

            other.GetComponent<Rigidbody>().useGravity = false;
            PlayerCharacter.transform.SetParent(CobwebHandle.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {

        GetComponent<Cobweb>().enabled = false;
        other.GetComponent<PlayerMove2>().enabled = true;
        other.GetComponentInChildren<Interaction>().enabled = true;
        other.GetComponent<Rigidbody>().useGravity = true;

        PlayerCharacter.transform.SetParent(other.transform);

    }
}

// 최대 각도 제한을 걸어두고 그 제한에 도달할 때마다 최대 각도를 조금씩 늘리는 식으로 가속 구현?
// 현 상태 최대 각도에 도달하면 그 때 조작 비활성화, 원위치로 돌아오기
