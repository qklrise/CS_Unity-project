using System.Collections.Generic;
using UnityEngine;

public class GameTime 
{
    static Dictionary<int, WaitForSeconds> waitList = new Dictionary<int, WaitForSeconds>();
    public static WaitForSeconds GetWait(float t)
    {
        int key = (int)(t * 100.0f);
        if (waitList.ContainsKey(key))
        {
            return waitList[key];
        }
        return waitList[key] = new WaitForSeconds(t);
    }
}
