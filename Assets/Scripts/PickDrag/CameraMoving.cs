using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] float rotaSpeed = 90.0f;
    [SerializeField] float moveSpeed = 5.0f;
    public Transform myCam;
    float angleX;
    float angleY;

    void Start()
    {
        //angleX = myCam.localRotation.x;
        //angleY = myCam.localRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        float delMoveSpeed = Time.deltaTime * moveSpeed;
        float delRotaSpeed = Time.deltaTime * rotaSpeed;
        float temp = Input.GetAxis("Horizontal") * delRotaSpeed;
        transform.Rotate(0,temp,0,Space.World);
        temp = 0;

        temp = Input.GetAxis("Vertical") * delMoveSpeed;
        transform.Translate(0, 0, temp, Space.World);
        temp = 0;

        if (Input.GetKey(KeyCode.E))
        {
            float delta = delMoveSpeed;
            temp += delta;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            float delta = delMoveSpeed;
            temp -= delta;
        }
        transform.Translate(0,temp,0, Space.World);

        temp = Input.GetAxis("Mouse ScrollWheel") *delMoveSpeed *90.0f;
        Vector3 cameraDel = transform.position - myCam.position;
        cameraDel.Normalize();
        myCam.Translate(cameraDel * temp, Space.World);

        if (Input.GetMouseButton(1))
        {
            float rotX, rotY;
            /*
            rotX = Input.GetAxis("Mouse Y") * delRotaSpeed * 30.0f;
            rotY = Input.GetAxis("Mouse X") * delRotaSpeed * 30.0f;
            angleX += rotX;
            angleY += rotY;
            myCam.localRotation = Quaternion.Euler(angleX, angleY, 0);
            */

            rotX = Input.GetAxis("Mouse Y") * delRotaSpeed * 30.0f;
            rotY = Input.GetAxis("Mouse X") * delRotaSpeed * 30.0f;
            myCam.Rotate(rotX, -rotY, 0,Space.Self);
        }
    }
}
