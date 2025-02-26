using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingBar;
    public TextMeshProUGUI progressText;
    public string sceneToLoad = "InGame";

    void Start()
    {
        StartCoroutine(FakeLoading());
    }

    IEnumerator FakeLoading()
    {
        float fakeProgress = 0f;
        float fakeLoadingTime = 5f; 
        float elapsedTime = 0f;

        while (elapsedTime < fakeLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            fakeProgress = Mathf.Clamp01(elapsedTime / fakeLoadingTime);
            loadingBar.value = fakeProgress;
            progressText.text = (fakeProgress * 100).ToString("F0") + "%";
            yield return null;
        }

        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            progressText.text = (progress * 100).ToString("F0") + "%";
            yield return null;
        }

        loadingBar.value = 1f;
        progressText.text = "Memuat game";

        yield return new WaitForSeconds(1f); 

        operation.allowSceneActivation = true;
    }
}
