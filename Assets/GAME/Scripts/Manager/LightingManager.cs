using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public static LightingManager Instance;

    public Light directionLight;
    public GameObject pointLight;

    [Header("Customizable Colors")]
    public Color defaultColor = Color.white;
    public Color hujanColor = new Color(0.6f, 0.6f, 0.6f);
    public Color soreColor = new Color(1f, 0.811f, 0.458f);
    public Color malamColor = new Color(0.1f, 0.1f, 0.2f);

    private float defaultIntensity;
    private bool isListrikMati = false;
    private bool isHujan = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (directionLight != null)
        {
            defaultIntensity = directionLight.intensity;
        }
    }

    public void UpdateLightingByTime(int hour, int minute)
    {
        if (isListrikMati) return;

        Color targetColor = defaultColor;
        float targetIntensity = defaultIntensity;

        if (isHujan)
        {
            targetColor = hujanColor;
            targetIntensity *= 0.8f;
        }
        else if (hour >= 14 && hour < 16)
        {
            pointLight.SetActive(false);
            targetColor = Color.white;
            targetIntensity = 1.0f;
        }
        else if (hour >= 16 && hour < 17)
        {
            targetColor = soreColor;
            targetIntensity = 1.0f;
        }
        else if (hour >= 17 && minute >= 30)
        {
            NotificationManager.Instance.ShowNotification("Sudah mau magrib, ayo pulang!");
            pointLight.SetActive(true);
            targetColor = malamColor;
            targetIntensity = 0.5f;
        }

        directionLight.color = targetColor;
        directionLight.intensity = targetIntensity;
    }

    public void SetListrikMati(bool status)
    {
        isListrikMati = status;
        
        if (status)
        {
            directionLight.intensity = 0.3f;
            pointLight.SetActive(false);
        }
        else
        {
            UpdateLightingByTime(TimeManager.Instance.currentHour, TimeManager.Instance.currentMinute);
        }
    }

    public void SetHujan(bool status)
    {
        isHujan = status;
        UpdateLightingByTime(TimeManager.Instance.currentHour, TimeManager.Instance.currentMinute);
    }
}
