using UnityEngine;

public class Cobweb2 : MonoBehaviour
{
    public GameObject CobwebHandle;

    float maxAngle;
    float minAngle;
    float curAngle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        maxAngle = CobwebHandle.GetComponent<HingeJoint>().limits.max;
        minAngle = CobwebHandle.GetComponent<HingeJoint>().limits.min;

        if (curAngle == maxAngle || curAngle == minAngle)
        {
            maxAngle = CobwebHandle.GetComponent<HingeJoint>().limits.max + 10.0f;
            minAngle = CobwebHandle.GetComponent<HingeJoint>().limits.min - 10.0f;
        }
    }
}
