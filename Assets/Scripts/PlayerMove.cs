using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Transform myModel;
    public float moveSpeed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 modelDir = myModel.localRotation * Vector3.forward;
        float angle = Vector3.Angle(modelDir, inputDir);
        float rotdir = Vector3.Dot(inputDir, myModel.localRotation * Vector3.right) < 0.0f ? -1.0f : 1.0f;

        float delta = Time.deltaTime * 720.0f;
        if (delta > angle) delta = angle;
        myModel.Rotate(Vector3.up * delta * rotdir);
        transform.position = transform.position + myModel.forward * inputDir.magnitude * Time.deltaTime * moveSpeed;
    }
}

