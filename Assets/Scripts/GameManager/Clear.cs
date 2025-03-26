using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    public LayerMask playerMask;
    public Collider doorCol;
    
    private void Start()
    {
        doorCol = gameObject.GetComponent<BoxCollider>();
        doorCol.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if((1<< other.gameObject.layer & playerMask) != 0)
        {
            int curSceanNum = SceneManager.GetActiveScene().buildIndex;
            LoadSystem.LoadScene(curSceanNum + 1);
        }
    }
}
