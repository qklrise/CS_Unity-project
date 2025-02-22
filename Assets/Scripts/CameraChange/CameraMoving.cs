using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] float rotaSpeed = 90.0f;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float minYaxis = 5.0f;
    [SerializeField] float maxYaxis = 8.0f;
    [SerializeField] float minZoom = 5.0f;
    [SerializeField] float maxZoom = 10.0f;
    public GameObject puzzleCam;
    Transform myCam;

    private void Start()
    {
        myCam = puzzleCam.transform;
    }
    void RotHor()
    {
        if (Input.GetAxis("Horizontal") != 0.0f)
        {
            float delRotaSpeed = Time.deltaTime * rotaSpeed;
            float temp = Input.GetAxis("Horizontal") * delRotaSpeed;
            transform.Rotate(0, temp, 0, Space.World);
        }
    }

    void MovVer()
    {
        if (Input.GetAxis("Vertical") != 0.0f)
        {
            float delMoveSpeed = Time.deltaTime * moveSpeed;
            float temp = Input.GetAxis("Vertical") * delMoveSpeed;
            myCam.Translate(0, 0, temp, Space.World);
        }
    }

    void MovZ()
    {
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))
        {
            float delMoveSpeed = Time.deltaTime * moveSpeed;
            float temp = 0.0f;
            float delta = delMoveSpeed;
            float offset = 0.5f;

            if (Input.GetKey(KeyCode.E))
            {
                if (Physics.Raycast(myCam.position, Vector3.up, out RaycastHit hit, offset + delta)) 
                    delta = hit.point.y - (myCam.position.y + offset);
                temp += delta;
            }
            else
            {
                if (Physics.Raycast(myCam.position, Vector3.down, out RaycastHit hit, offset + delta))
                    delta = myCam.position.y - (offset + hit.point.y);
                temp -= delta;
            }

            myCam.Translate(Vector3.up * temp, Space.World);
        }
    }

    void MovWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            float delMoveSpeed = Time.deltaTime * moveSpeed;
            float temp = Input.GetAxis("Mouse ScrollWheel") * delMoveSpeed * 90.0f;
            float cameradist = Mathf.Abs(myCam.localPosition.z);
            cameradist = Mathf.Clamp(cameradist - temp, minZoom, maxZoom);
            myCam.localPosition = new(0, 0, -cameradist);
        }
    }

    void RotRM()
    {
        if (Input.GetMouseButton(1))
        {
            float delRotaSpeed = Time.deltaTime * rotaSpeed;
            float rotX, rotY;
            rotX = Input.GetAxis("Mouse Y") * delRotaSpeed * 10.0f;
            rotY = Input.GetAxis("Mouse X") * delRotaSpeed * 10.0f;
            myCam.Rotate(rotX, -rotY, 0, Space.World);
        }
    }
    void Update()
    {
        if (puzzleCam.activeInHierarchy)
        {
            RotHor();
            MovVer();
            MovZ();
            MovWheel();
            RotRM();
        }
    }
}
