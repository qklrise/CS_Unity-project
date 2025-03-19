using UnityEngine;

public class InWaterMove : MonoBehaviour
{
    public LayerMask playerMask;
    PlayerMove2 pm = null;
    [SerializeField] float waterMoveSpeed = 0.5f;
    private void OnTriggerEnter(Collider other)
    {
        if((1<<other.gameObject.layer & playerMask) != 0)
        {
            if(pm == null)pm = other.gameObject.GetComponent<PlayerMove2>();
            pm.SetMaxSpeed(waterMoveSpeed);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & playerMask) != 0)
        {
            if (pm == null) pm = other.gameObject.GetComponent<PlayerMove2>();
            pm.SetMaxSpeed(1.0f);
        }
    }
}
