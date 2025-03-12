using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    public LayerMask playerMask;

    private void OnTriggerEnter(Collider other)
    {
        if((1<< other.gameObject.layer & playerMask) != 0)
        {
            int curSceanNum = SceneManager.GetActiveScene().buildIndex;
            LoadSystem.LoadScene(curSceanNum + 1);
        }
    }
}
