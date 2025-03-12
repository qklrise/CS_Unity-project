using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSystem : MonoBehaviour
{
    public GameObject infoText;
    public GameObject endText;
    public Slider mySlider;
    public GameObject myFill;
    static int targetScene;

    public static void LoadScene(int n)
    {
        targetScene = n;
        SceneManager.LoadScene(0);
    }

    void Start()
    {
        StartCoroutine(LoadingScene());
    }

    IEnumerator LoadingScene()
    {
        endText.SetActive(false);
        mySlider.value = 0f;
        AsyncOperation ao = SceneManager.LoadSceneAsync(targetScene);
        ao.allowSceneActivation = false;

        while (mySlider.value < mySlider.maxValue)
        {
            yield return StartCoroutine(UpdateSlider(ao.progress));
        }

        infoText.SetActive(false);
        endText.SetActive(true);

        while (true)
        {
            if (Input.anyKeyDown)
            {
                break;
            }
            yield return null;
        }
        ao.allowSceneActivation = true;
    }
    IEnumerator UpdateSlider(float v)
    {
        while (mySlider.value < v)
        {
            if (mySlider.value < 0.3f) myFill.GetComponent<Image>().color = Color.red;
            else if (mySlider.value < 0.6f) myFill.GetComponent<Image>().color = Color.yellow;
            else myFill.GetComponent<Image>().color = Color.green;
            mySlider.value += Time.deltaTime;
            yield return null;
        }
        mySlider.value = v;
    }
}
