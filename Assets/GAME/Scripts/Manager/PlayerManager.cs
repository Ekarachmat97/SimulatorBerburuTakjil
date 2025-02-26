using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [Header("Upgrade Settings")]
    [Tooltip("jika nilai movement speed ini di rubah, jangan lupa di Player nya juga rubah, samakan")]
    public float movementSpeed = 5f;
    public float timeModifier = 1f; 
    public float discountBonus = 0f;
    public int bonusCoin = 0;

    [Header("Player Currency")]
    public int totalCoins = 0;
    public int totalPahala = 0;
    public int playerLevel = 1;
    public int currentLevel;
    public int pahalaThreshold = 100; 
    public float thresholdMultiplier = 1.5f; 

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI levelText;
    public Slider pahalaSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentLevel = playerLevel;
        UpdateUI();
    }

    void Update()
    {
         UpdateUI();
    }

    public void AddCoins(int amount)
    {
        AudioManager.instance.Play("collectmoney");
        totalCoins += amount;
        UpdateUI();
    }

    public void SubtractCoins(int amount)
    {
        totalCoins -= amount;
        UpdateUI();
    }

    public void AddPahala(int amount)
    {
        totalPahala += amount;

        while (totalPahala >= pahalaThreshold)
        {
            totalPahala -= pahalaThreshold;
            playerLevel++;
            pahalaThreshold = Mathf.RoundToInt(pahalaThreshold * thresholdMultiplier); 

            AudioManager.instance.Play("levelup");
            NotificationManager.Instance.ShowNotification("Level Naik ke " + playerLevel);

            LapakUnlockManager.Instance.UpdateLapak();
        }

        UpdateUI();
    }

    public void SubtractPahala(int amount)
    {
        totalPahala -= amount;

        while (totalPahala <= 0 && playerLevel > 1)
        {
            playerLevel--; 
            pahalaThreshold = GetPahalaThreshold(playerLevel); 
            totalPahala = pahalaThreshold - 1; 
        }

        if (playerLevel == 1 && totalPahala < 0) 
        {
            totalPahala = 0;
        }

        UpdateUI();
    }

    public int GetPahalaThreshold(int level)
    {
        return Mathf.RoundToInt(100 * Mathf.Pow(thresholdMultiplier, level - 1)); 
    }

    public void UpdateUI()
    {
        if (coinText != null)
            coinText.text = FormatCoins(totalCoins); 

        if (levelText != null)
            levelText.text = "Lv" + playerLevel;

        if (pahalaSlider != null)
        {
            pahalaSlider.maxValue = pahalaThreshold;
            pahalaSlider.value = totalPahala;
        }
    }

    private string FormatCoins(int amount)
    {
        if (amount >= 1000000)
            return (amount / 1000000f).ToString("0.#") + "M"; 
        else if (amount >= 1000)
            return (amount / 1000f).ToString("0.#") + "K";
        else
            return amount.ToString(); 
    }
}
