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

    private int currentPrice;
    private string selectedSkill = "";

    [Header("Harga Untuk Upgrade")]
    [SerializeField] private int basePriceMovement = 15000;
    [SerializeField] private int basePriceTime = 10000;
    [SerializeField] private int basePriceMenawar = 20000;
    [SerializeField] private int basePriceBonusCoin = 12000;

    private int maxUpgradeLevel = 10;
    private int levelMovement = 1;
    private int levelTime = 1;
    private int levelMenawar = 1;
    private int levelBonusCoin = 1;

    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Start()
    {
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
        int currentLevel = GetSkillLevel(selectedSkill);
        if (currentLevel >= maxUpgradeLevel)
        {
            statsText.text = "Skill ini sudah mencapai level maksimal!";
            priceText.text = "";
            upgradeButton.interactable = false;
            return;
        }
        
        upgradeButton.interactable = true;

        switch (selectedSkill)
        {
            case "MovementSpeed":
                currentPrice = basePriceMovement * currentLevel;
                statsText.text = $"Movement Speed: {playerManager.movementSpeed:F1} → {playerManager.movementSpeed + 0.2f:F1}\nLevel {currentLevel}/10";
                break;
            case "Time":
                currentPrice = basePriceTime * currentLevel;
                statsText.text = $"Waktu: {playerManager.timeModifier:F2} → {playerManager.timeModifier - 0.01f:F2}\nLevel {currentLevel}/10";
                break;
            case "Menawar":
                currentPrice = basePriceMenawar * currentLevel;
                statsText.text = $"Diskon Menawar: {playerManager.discountBonus * 100}% → {(playerManager.discountBonus + 0.05f) * 100}%\nLevel {currentLevel}/10";
                break;
            case "BonusCoin":
                currentPrice = basePriceBonusCoin * currentLevel;
                statsText.text = $"Bonus Coin: {playerManager.bonusCoin} → {playerManager.bonusCoin + 500}\nLevel {currentLevel}/10";
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

        int currentLevel = GetSkillLevel(selectedSkill);
        if (currentLevel >= maxUpgradeLevel)
        {
            Debug.Log("Skill sudah mencapai level maksimal!");
            return;
        }

        if (playerManager.totalCoins >= currentPrice)
        {
            playerManager.totalCoins -= currentPrice;

            switch (selectedSkill)
            {
                case "MovementSpeed":
                    playerManager.movementSpeed += 0.2f;
                    levelMovement++;
                    break;

                case "Time":
                    playerManager.timeModifier -= 0.01f;
                    levelTime++;
                    break;

                case "Menawar":
                    playerManager.discountBonus += 0.05f;
                    levelMenawar++;
                    break;

                case "BonusCoin":
                    playerManager.bonusCoin += 500;
                    levelBonusCoin++;
                    break;
            }

            Debug.Log($"Upgrade {selectedSkill} berhasil! Harga: {currentPrice}");

            UpdateStatsText();
        }
        else
        {
            AudioManager.instance.Play("close");
            Debug.Log("Uang tidak cukup untuk upgrade!");
        }
    }

    private int GetSkillLevel(string skill)
    {
        return skill switch
        {
            "MovementSpeed" => levelMovement,
            "Time" => levelTime,
            "Menawar" => levelMenawar,
            "BonusCoin" => levelBonusCoin,
            _ => 1
        };
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
        statsText.text = "";
        priceText.text = "";
        upgradePanel.SetActive(false);
    }
}
