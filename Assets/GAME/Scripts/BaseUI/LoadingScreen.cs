using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingSlider;
    public float loadingDuration = 3f;
    public TextMeshProUGUI loadingMessageText;

    [Header("Pesan Fun Fact")]
    public string[] foodFacts = new string[]
    {
        "Kolak candil berasal dari Jawa dan sering disajikan saat Ramadhan.",
        "Kurma adalah makanan favorit saat berbuka karena tinggi energi.",
        "Es timun suri populer di Indonesia sebagai minuman berbuka.",
        "Bubur kampiun berasal dari Minangkabau dan khas di bulan Ramadhan.",
        "Martabak manis sering dijadikan camilan berbuka puasa.",
        "Sop buah adalah hidangan segar yang biasa disajikan saat berbuka.",
        "Kue lapis legit sering dihidangkan saat Ramadhan dan Lebaran.",
        "Takjil berasal dari bahasa Arab yang berarti menyegerakan berbuka."
    };

    private void Start()
    {
        Time.timeScale = 0;
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }

    public void StartLoading()
    {
        if (loadingPanel != null && loadingSlider != null)
        {
            loadingPanel.SetActive(true);
            SetRandomFoodFact();
            StartCoroutine(FakeLoading());
        }
    }

    private void SetRandomFoodFact()
    {
        if (loadingMessageText != null && foodFacts.Length > 0)
        {
            int randomIndex = Random.Range(0, foodFacts.Length);
            loadingMessageText.text = foodFacts[randomIndex];
        }
    }

    private IEnumerator FakeLoading()
    {
        float startTime = Time.realtimeSinceStartup; 
        float timer = 0f;
        loadingSlider.value = 0f;

        while (timer < loadingDuration)
        {
            timer = Time.realtimeSinceStartup - startTime; 
            loadingSlider.value = timer / loadingDuration;
            yield return null;
        }

        loadingPanel.SetActive(false);
        TimeManager.Instance.DirectNextDay();
    }
}
