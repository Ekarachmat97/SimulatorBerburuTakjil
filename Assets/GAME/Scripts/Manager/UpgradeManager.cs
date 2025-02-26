using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public PlayerManager playerManager;

    public GameObject upgradePanel;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI priceText;

    [Header("Button")]
    public Button skillMovement;
    public Button skillTime;
    public Button skillMenawar;
    public Button skillBonusCoins;
    public Button upgradeButton;
    public Button openUpgradePanel;
    public Button closeUpgradePanel;

    private int upgradeLevel = 1;
    private int currentPrice;
    private string selectedSkill = "";

    [Header("Harga Untuk Upgrade")]
    [SerializeField] private int basePriceMovement = 15000;
    [SerializeField] private int basePriceTime = 10000;
    [SerializeField] private int basePriceMenawar = 20000;
    [SerializeField] private int basePriceBonusCoin = 12000;

    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Start()
    {
        // Tambahkan event listener untuk tombol skill
        skillMovement.onClick.AddListener(() => ShowUpgradeInfo("MovementSpeed"));
        skillTime.onClick.AddListener(() => ShowUpgradeInfo("Time"));
        skillMenawar.onClick.AddListener(() => ShowUpgradeInfo("Menawar"));
        skillBonusCoins.onClick.AddListener(() => ShowUpgradeInfo("BonusCoin"));

       
        upgradeButton.onClick.AddListener(UpgradeSkill);
        closeUpgradePanel.onClick.AddListener(() => ClosePanelUpgrade());
        openUpgradePanel.onClick.AddListener(() => OpenUpgradePanel());
    }

    private void ShowUpgradeInfo(string skill)
    {
        selectedSkill = skill;
        UpdateStatsText();
    }

    private void UpdateStatsText()
    {
        switch (selectedSkill)
        {
            case "MovementSpeed":
                currentPrice = basePriceMovement * upgradeLevel;
                statsText.text = $"Movement Speed: {playerManager.movementSpeed:F1} → {playerManager.movementSpeed + 0.2f:F1}";
                break;
            case "Time":
                currentPrice = basePriceTime * upgradeLevel;
                statsText.text = $"Waktu: {playerManager.timeModifier:F2} → {playerManager.timeModifier - 0.01f:F2}";
                break;
            case "Menawar":
                currentPrice = basePriceMenawar * upgradeLevel;
                statsText.text = $"Diskon Menawar: {playerManager.discountBonus * 100}% → {(playerManager.discountBonus + 0.05f) * 100}%";
                break;
            case "BonusCoin":
                currentPrice = basePriceBonusCoin * upgradeLevel;
                statsText.text = $"Bonus Coin: {playerManager.bonusCoin} → {playerManager.bonusCoin + 500}";
                break;
        }

        priceText.text = $"Harga: {currentPrice}";
    }

    private void UpgradeSkill()
    {
        if (selectedSkill == "")
        {
            AudioManager.instance.Play("close");
            
            return;
        }

        if (playerManager.totalCoins >= currentPrice)
        {
            playerManager.totalCoins -= currentPrice;

            switch (selectedSkill)
            {
                case "MovementSpeed":
                    playerManager.movementSpeed += 0.2f;
                    if (FindObjectOfType<PlayerMovement>() != null)
                    {
                        FindObjectOfType<PlayerMovement>().moveSpeed = playerManager.movementSpeed;
                    }
                    break;

                case "Time":
                    playerManager.timeModifier += 0.02f; 
                    if (FindObjectOfType<TimeManager>() != null)
                    {
                        FindObjectOfType<TimeManager>().UpdateTimeSpeed();
                    }
                    break;

                case "Menawar":
                    playerManager.discountBonus += 5f; // Tambah 5% diskon tiap upgrade
                    if (FindObjectOfType<GenericStore>() != null)
                    {
                        FindObjectOfType<GenericStore>().UpdatePlayerDiscountBonus();
                    }
                    break;

                case "BonusCoin":
                    playerManager.bonusCoin += 500; // Tambah 500 koin tiap upgrade
                    if (FindObjectOfType<RewardUI>() != null)
                    {
                        FindObjectOfType<RewardUI>().UpdatePlayerBonusCoins();
                    }
                    break;
            }

            Debug.Log($"Upgrade {selectedSkill} berhasil! Harga: {currentPrice}");

            upgradeLevel++;
            UpdateStatsText();
        }
        else
        {
            AudioManager.instance.Play("close");

            Debug.Log("Uang tidak cukup untuk upgrade!");
        }
    }

    private void OpenUpgradePanel()
    {
        Time.timeScale = 0;
        AudioManager.instance.Play("clicksfx");
        upgradePanel.SetActive(true);
    }

    private void ClosePanelUpgrade()
    {
        Time.timeScale = 1;
        AudioManager.instance.Play("close");

        statsText.text="";
         priceText.text = $"";
        upgradePanel.SetActive(false);
    }
}
