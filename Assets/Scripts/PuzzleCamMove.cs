using UnityEngine;

public class PuzzleCamMove : MonoBehaviour
{
    [SerializeField] float rotSpeed = 1.0f;
    [SerializeField] float smoothSpeed = 1.0f;

    public float moveSpeed = 5.0f;
    float rotX, rotY, targetRotX, targetRotY;
    public Rigidbody rig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            //카메라 rotation
            float temp = -Input.GetAxis("Mouse Y") * rotSpeed;
            targetRotX += temp;

            float temp2 = Input.GetAxis("Mouse X") * rotSpeed;
            targetRotY += temp2;


            rotX = Mathf.Lerp(rotX, targetRotX, Time.deltaTime * smoothSpeed);
            rotY = Mathf.Lerp(rotY, targetRotY, Time.deltaTime * smoothSpeed);

            transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
        }
        //카메라 position
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(inputDir * Time.deltaTime * moveSpeed,Space.Self);
       /* if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            float delMoveSpeed = Time.deltaTime * moveSpeed;
            float temp3 = 0.0f;
            float delta = delMoveSpeed;
            float offset = 0.5f;

            if (Input.GetKey(KeyCode.W))
            {
                if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, offset + delta))
                    delta = hit.point.z - (transform.position.z + offset);
                temp3 += delta;
            }
            else
            {
                if (Physics.Raycast(transform.position, Vector3.back, out RaycastHit hit, offset + delta))
                    delta = transform.position.z - (offset + hit.point.z);
                temp3 -= delta;
            }

            transform.Translate(Vector3.forward * temp3, Space.World);
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float delMoveSpeed = Time.deltaTime * moveSpeed;
            float temp3 = 0.0f;
            float delta = delMoveSpeed;
            float offset = 0.5f;

            if (Input.GetKey(KeyCode.D))
            {
                if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit hit, offset + delta))
                    delta = hit.point.x - (transform.position.x + offset);
                temp3 += delta;
            }
            else
            {
                if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit, offset + delta))
                    delta = transform.position.x - (offset + hit.point.x);
                temp3 -= delta;
            }

            transform.Translate(Vector3.right * temp3, Space.World);
        }*/
        
        //q,e
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))
        {
            float delMoveSpeed = Time.deltaTime * moveSpeed;
            float temp3 = 0.0f;
            float delta = delMoveSpeed;
            float offset = 0.5f;

            if (Input.GetKey(KeyCode.E))
            {
                if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, offset + delta))
                    delta = hit.point.y - (transform.position.y + offset);
                temp3 += delta;
            }
            else
            {
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, offset + delta))
                    delta = transform.position.y - (offset + hit.point.y);
                temp3 -= delta;
            }

            transform.Translate(Vector3.up * temp3, Space.World);
        }
        //rig.linearVelocity = Vector3.zero;
    }
}
