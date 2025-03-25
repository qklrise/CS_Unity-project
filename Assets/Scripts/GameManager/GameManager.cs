using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static bool isPuzzle = false;
   public static bool isDrag = false;

   public static void ChangeGameMode()
   {
       if (!isDrag) isPuzzle = !isPuzzle;
   }
}