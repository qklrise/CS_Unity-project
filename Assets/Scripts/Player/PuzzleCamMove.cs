using UnityEngine;

public class PuzzleCamMove : MonoBehaviour
{
    [SerializeField] float rotSpeed = 1.0f;
    [SerializeField] float smoothSpeed = 1.0f;
    public float Setting = 0.0f;   
    public float moveSpeed =0f;
    float rotX, rotY, targetRotX, targetRotY;
    public Rigidbody rig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
   
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rig.isKinematic = true;
        if(Input.GetMouseButton(1))
        {
            CameraRot();
            rig.isKinematic = false;
        }
        //카메라 position
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputDir = transform.rotation * inputDir;
        inputDir *= Time.fixedDeltaTime * moveSpeed;
        //transform.Translate(inputDir * Time.smoothDeltaTime * moveSpeed,Space.Self);
       /*if (Input.GetKey(KeyCode.W))
        {
            float delMoveSpeed = Time.deltaTime * moveSpeed;
            float temp3 = 0.0f;
            float delta = delMoveSpeed;
            float offset = 0.5f;
            if(Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, offset + delta))
            {
                delta = hit.point.z - (transform.position.z + offset);
            }
                temp3 += delta;

            if (Physics.Raycast(transform.position, Vector3.back, out RaycastHit hit2, offset + delta))
            {
                delta = transform.position.z - (offset + hit2.point.z);
                temp3 -= delta;
            }
            if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit hit3, offset + delta))
            {
                delta = hit3.point.x - (transform.position.x + offset);
                temp3 += delta;
            }
            if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit4, offset + delta))
            {
                delta = transform.position.x - (offset + hit4.point.x);
                temp3 -= delta;
            }
        }*/
       
        //if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))
        //{
            float delMoveSpeed = Time.fixedDeltaTime * moveSpeed;
            float temp3 = 0.0f;
            float delta = delMoveSpeed;
            float offset = 0.5f;

            if (Input.GetKey(KeyCode.E))
            {
                if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, offset + delta))
                    delta = hit.point.y - (transform.position.y + offset);
                temp3 += delta;
            }
            else if(Input.GetKey(KeyCode.Q))
            {
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, offset + delta))
                    delta = transform.position.y - (offset + hit.point.y);
                temp3 -= delta;
            }

            //transform.Translate(Vector3.up * temp3, Space.World);
        //}
        transform.Translate(inputDir + Vector3.up * temp3, Space.World);
    }
    private void CameraRot()
    {
        //카메라 rotation
            float temp = -Input.GetAxis("Mouse Y") * rotSpeed;
            targetRotX += temp;

            float temp2 = Input.GetAxis("Mouse X") * rotSpeed;
            targetRotY += temp2;


            rotX = Mathf.Lerp(rotX, targetRotX, Time.deltaTime * smoothSpeed);
            rotY = Mathf.Lerp(rotY, targetRotY, Time.deltaTime * smoothSpeed);

            transform.localRotation = Quaternion.Euler(rotX + Setting, rotY, 0);
    }
    void OnCollisionEnter(Collision collision)
    {
        
        
    }
    /*public float cameraSetting(float a)
    {
        Setting = a;
        return Setting;
    }*/

}

