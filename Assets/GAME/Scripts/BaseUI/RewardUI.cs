using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardUI : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject rewardUIPanel;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI evaluationText;

    [Header("Pesan evaluasi")]
     [SerializeField] private string highEvaluationMessage = "Luar biasa! Kamu sangat dermawan!";
    [SerializeField] private string mediumEvaluationMessage = "Bagus sekali! Kamu mulai menjadi panutan.";
    [SerializeField] private string lowEvaluationMessage = "Kerja bagus! Lanjutkan usahamu!";
    [SerializeField] private string failEvaluationMessage = "Terus berusaha! Pahala itu berharga.";
    
     [Header("reward setting")]
    private int playerBonusCoins = 0; // Bonus koin tambahan dari upgrade

    public int initialRewardAmount = 15000;
    public int questRewardAmount = 0;
    public int rewardAmount = 1000;
    public int initialPahalaAmount = 100;
    
    private PlayerManager playerManager;
    private LoadingScreen loadingScreen;
    private TakjilQuestManager questManager;

    public GameObject starsPanel;
    public GameObject noStarsPanel;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        loadingScreen = FindObjectOfType<LoadingScreen>();
        questManager = FindObjectOfType<TakjilQuestManager>();

        UpdatePlayerBonusCoins();
    }

    public void OpenRewardUI()
    {
        AudioManager.instance.Play("getreward");
        rewardUIPanel.SetActive(true);
        Time.timeScale = 0;

        TimeManager timeManager = FindObjectOfType<TimeManager>();

        if (timeManager.questCompleted)
        {
            questManager.questCompletedCount++; 
            ScaleReward(); // Atur reward berdasarkan progres
            ShowEvaluationCompleted();
            GiveReward(questRewardAmount);
        }
        else
        {
            ShowEvaluationFailed();
        }
    }

    private void ScaleReward()
    {
        // Skala reward berdasarkan jumlah quest yang selesai
        questRewardAmount = rewardAmount + (questManager.questCompletedCount * 500);
        initialPahalaAmount = 100 + (questManager.questCompletedCount * 100);
    }


    public void CloseRewardUI()
    {
        AudioManager.instance.Play("close");
        Time.timeScale = 0;
        rewardUIPanel.SetActive(false);
    }

    public void GiveReward(int amount)
    {
        if (playerManager == null) return;

        int totalReward = amount + playerBonusCoins; // Tambahkan bonus koin dari upgrade
        playerManager.AddCoins(totalReward);

        if (rewardText != null)
        {
            rewardText.text = $"Kamu mendapatkan {totalReward} koin (+{playerBonusCoins} bonus)!\n" +
                            $"Kamu mendapatkan {initialPahalaAmount} pahala!";
        }
    }

    public void UpdatePlayerBonusCoins()
    {
        if (playerManager != null)
        {
            playerBonusCoins = playerManager.bonusCoin; // Ambil nilai bonus koin dari PlayerManager
        }
    }



    private void ShowEvaluationCompleted()
    {
        if (evaluationText == null || playerManager == null) return;

        int totalPahala = playerManager.totalPahala;
        string evaluationMessage = "";

        if (totalPahala >= 1000)
            evaluationMessage = highEvaluationMessage;
        else if (totalPahala >= 500)
            evaluationMessage = mediumEvaluationMessage;
        else if (totalPahala >= 250)
            evaluationMessage = lowEvaluationMessage;
        else
            evaluationMessage = failEvaluationMessage;

        evaluationText.text = evaluationMessage;

        starsPanel.SetActive(true);
        noStarsPanel.SetActive(false);
    }

    public void ShowEvaluationFailed()
    {
        if (evaluationText == null || playerManager == null) return;

        evaluationText.text = "Sayang sekali, kamu belum menyelesaikan quest hari ini. Coba lagi besok ya!";
        rewardText.text = "Tidak ada reward kali ini. Tetap semangat!";

        // Penalti: 50% dari total pahala
        int penalty = Mathf.RoundToInt(playerManager.totalPahala * 0.5f);
        playerManager.SubtractPahala(penalty);

        // Cek jika pahala sekarang <= 0, maka level turun
        if (playerManager.totalPahala <= 0)
        {
            if (playerManager.currentLevel > 1) // Cek biar level tidak turun di bawah level 1
            {
                playerManager.currentLevel--; // Turunkan level
            }
            else
            {
                playerManager.totalPahala = 0; // Kalau sudah level 1, pahala tetap 0
            }
        }

        starsPanel.SetActive(false);
        noStarsPanel.SetActive(true);
    }


    public void GoToNextDay()
    {
        Time.timeScale = 1;
        loadingScreen.StartLoading();
    }
}
