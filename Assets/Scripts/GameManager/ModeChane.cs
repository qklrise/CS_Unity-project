using UnityEngine;

public class ModeChane : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (GameManager.isPuzzle)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }

            else
            {
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
