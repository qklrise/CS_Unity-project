using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LayerMask crashMask;
    [SerializeField] float rotSpeed = 1.0f;
    [SerializeField] float zoomSpeed = 1.0f;
    [SerializeField] Vector2 zoomRange = new Vector2(1.0f, 10.0f);
    [SerializeField] float smoothSpeed = 1.0f;
    [SerializeField] Transform Player;
    public Transform myCam;
    float camDist = 0.0f;
    float targetDist;    
    float rotX, rotY, targetRotX, targetRotY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetDist = camDist = Mathf.Abs(myCam.transform.localPosition.z);
        rotX = transform.localRotation.eulerAngles.x;
        if (rotX > 180.0f) rotX -= 360.0f;
        targetRotX = rotX;
        targetRotY = rotY = transform.parent.localRotation.eulerAngles.y;        
    }

    // Update is called once per frame
    void Update()
    {
        
        float temp = -Input.GetAxis("Mouse Y") * rotSpeed;
        targetRotX = Mathf.Clamp(targetRotX + temp, -50.0f, 80.0f);           

        temp = Input.GetAxis("Mouse X") * rotSpeed;
        targetRotY += temp;
        

        rotX = Mathf.Lerp(rotX, targetRotX, Time.deltaTime * smoothSpeed);
        rotY = Mathf.Lerp(rotY, targetRotY, Time.deltaTime * smoothSpeed);

        //Vector3 lookforward = new Vector3(myCam.forward.x, 0, myCam.forward.z).normalized;
        //Player.forward = lookforward;

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.parent.localRotation = Quaternion.Euler(0, rotY, 0);

        float delta = Input.GetAxis("Mouse ScrollWheel");
        
            targetDist = Mathf.Clamp(targetDist + delta * zoomSpeed, zoomRange.x, zoomRange.y);
            camDist = Mathf.Lerp(camDist, targetDist, Time.deltaTime * smoothSpeed);
            myCam.localPosition = new Vector3( 0, 0, -camDist );
        

        float offset = 0.5f;
        if(Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit,
            camDist  +offset, crashMask))
        {
            myCam.position = hit.point+ transform.forward * offset;
            camDist = hit.distance - offset;
        }        
    }
}
