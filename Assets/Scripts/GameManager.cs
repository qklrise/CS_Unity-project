using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static bool isPuzzle = false;
   public static bool isDrag = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isDrag == false)
        {
            isPuzzle = !isPuzzle;
        }
    }
}