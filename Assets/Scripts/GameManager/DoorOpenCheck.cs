using UnityEngine;

public class DoorOpenCheck : MonoBehaviour
{
    public Collider goalCol;

    public void OpenCheck()
    {
        goalCol.enabled = !goalCol.enabled;
    }
}
