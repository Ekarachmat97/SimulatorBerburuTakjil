using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MomDialogAndInteraction : MonoBehaviour, IInteractable
{
    [Header("UI Components")]
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public Button nextButton;
    public Button okButton;

    [Header("Dialog Messages")]
    public string[] initialDialogMessage = new string[] { "Kamu masih puasakan nak?", "Nak, ibu gak masak, tolong beli makanan yah." };
    public string[] questInProgressMessage = new string[] { "Apakah ada yang bisa ibu bantu nak?", "Nak kamu sudah pulang?", "Nak jangan lupa titipan ibu yah"};
    public string angryDialogMessage = "Ibu marah! Kamu tidak membawa takjil yang ibu minta!";
    public string completedDialogMessage = "Wah terima kasih yah, kembaliannya buat kamu!";
    public string waktunyaBerbukaDialogMessage = "Nak sudah waktunya berbuka, ayo pulang";

   
    private int dialogStage = 0;
    private TakjilQuestManager questManager;
    private RewardUI rewardUI;
    private PlayerManager playerManager;
    private QuestUI questUI;
    private TimeManager timeManager;
    [SerializeField] private bool isQuestRewardGiven = false;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        questManager = FindObjectOfType<TakjilQuestManager>();
        rewardUI = FindObjectOfType<RewardUI>();
        questUI = FindObjectOfType<QuestUI>();
        timeManager = FindObjectOfType<TimeManager>();

        if (nextButton != null) nextButton.onClick.AddListener(OnNextButtonClicked);
        if (okButton != null) okButton.onClick.AddListener(OnOkButtonClicked);

        ShowInitialDialog();
    }

    public void Interact()
    {
        if (IsAfterBerbukaTime() && questManager.IsQuestCompleted()) 
        {
            ShowBerbukaDialog();
            
        }
        else if (questManager.IsQuestCompleted()) 
        {
            if (!isQuestRewardGiven)
            {
                questUI.UpdateQuestUI();
                playerManager.AddPahala(rewardUI.initialPahalaAmount);
                ClearInventoryForQuest();
                ShowCompletedDialog();
                isQuestRewardGiven = true;
                timeManager.CompleteQuest();
            }
            else
            {
                ShowQuestInProgressDialog();
                
            }
        }
        else if (questManager.IsQuestInProgress()) 
        {
            ShowQuestInProgressDialog();
        }
        else 
        {
            ShowInitialDialog();
        }
    }

    private string GetRandomMomDialog()
    {
        if (initialDialogMessage.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, initialDialogMessage.Length);
            return initialDialogMessage[randomIndex];
        }
        return "Nak, ibu ingin bicara.";
    }

    private string GetRandomMomInProgressDialog()
    {
        if (questInProgressMessage.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, questInProgressMessage.Length);
            return questInProgressMessage[randomIndex];
        }
        return "Nak, ibu ingin bicara.";
    }




    private void UpdateDialogText()
    {
        switch (dialogStage)
        {
            case 0:
                dialogText.text = GetRandomMomDialog();
                break;
            case 1:
                dialogText.text = "Kamu pasti mau berburu takjilkan? Kalau iya, ibu nitip yah.";
                break;
            case 2:
                questUI?.ShowQuestPanel();
                break;
            case 3:
                dialogText.text = "Ini ibu berikan uang untuk belanjanya, " + rewardUI.initialRewardAmount + ".";
                playerManager.AddCoins(rewardUI.initialRewardAmount);
                break;
            case 4:
                CloseDialogPanel();
                break;
            default:
                break;
        }
    }

    private bool IsAfterBerbukaTime()
    {
        return timeManager.currentHour >= timeManager.questFailHour;
    }

    public void ShowBerbukaDialog()
    {
        ShowDialog(waktunyaBerbukaDialogMessage);
    }

    public void ShowInitialDialog()
    {
        Time.timeScale = 0;
        dialogPanel.SetActive(true);
        isQuestRewardGiven = false;
        dialogStage = 0;
        UpdateDialogText();
    }

    private void ShowQuestInProgressDialog()
    {
        Time.timeScale = 0;
        ShowDialog(GetRandomMomInProgressDialog()); // Perbaikan
    }


    public void ShowAngryDialog()
    {
        ShowDialog(angryDialogMessage);
    }

    public void ShowCompletedDialog()
    {
        ShowDialog(completedDialogMessage);
    }

    private void ShowDialog(string message)
    {
        
        if (dialogPanel != null && dialogText != null)
        {
            dialogPanel.SetActive(true);
            dialogText.text = message;
        }
    }

    

    private void OnNextButtonClicked()
    {
        AudioManager.instance.Play("clicksfx");

        if (dialogText.text == completedDialogMessage || 
            System.Array.Exists(questInProgressMessage, message => message == dialogText.text))
        {
            CloseDialogPanel();
            Time.timeScale = 1;
        }
        else
        {
            dialogStage++;
            UpdateDialogText();
        }
    }


    private void OnOkButtonClicked()
    {
        AudioManager.instance.Play("clicksfx");
        questUI?.HideQuestPanel();
        dialogStage++;
        UpdateDialogText();
    }

    private void CloseDialogPanel()
    {
        Time.timeScale = 1;
        if (dialogPanel != null) dialogPanel.SetActive(false);
    }

    private void ClearInventoryForQuest()
    {
        InventoryManager.Instance?.ClearInventoryForQuest(questManager.todayTakjilList);
    }

    public void ResetQuestForNextDay()
    {
        isQuestRewardGiven = false;
        // questManager.ResetDailyQuest();
    }
}
