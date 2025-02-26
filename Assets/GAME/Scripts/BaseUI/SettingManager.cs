using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public GameObject panelSetting; 
    public Slider volumeSlider;
    public Button exitButton; 
    public Button backButton; 

    private void Start()
    {
        panelSetting.SetActive(false);

        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);

        exitButton.onClick.AddListener(ExitGame);

        backButton.onClick.AddListener(CloseSettings);
    }

    private void SetVolume(float volume)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(volume);
            PlayerPrefs.SetFloat("Volume", volume);
        }
    }

    public void OpenSettings()
    {
        AudioManager.instance.Play("clicksfx");
        panelSetting.SetActive(true);
        Time.timeScale = 0; 
    }

    private void CloseSettings()
    {
       AudioManager.instance.Play("close");
        panelSetting.SetActive(false);
        Time.timeScale = 1;
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #else
        Application.Quit(); 
        #endif
    }

}
