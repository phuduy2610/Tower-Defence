using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    [SerializeField]
    private GameObject LoadScene;
    [SerializeField]
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLoadScene()
    {
        StartCoroutine(LoadSceneAsync("Gameplay"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        LoadScene.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
