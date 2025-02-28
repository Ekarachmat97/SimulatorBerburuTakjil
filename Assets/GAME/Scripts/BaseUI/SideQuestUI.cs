using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideQuestUI : MonoBehaviour
{
    public static SideQuestUI Instance;

    [Header("UI Components")]
    public Button closeButton;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questProgressText;
    public TextMeshProUGUI questTimerText;
    public TextMeshProUGUI dialogText;
    public GameObject npcSideQuestDialogPanel;
    [Header("UI Dialogs")]
    public string[] dialogMessage = new string[] 
    {
        "Aku kehilangan barangku, bantu aku carikan yah",
        "Tolong, nak, barang ku hilang di sekitar sini",
        "aku sudah cape mencari barangku yang hilang, tolong bantu aku yah"
    };
    public GameObject questPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        if (closeButton != null) closeButton.onClick.AddListener(CloseDialog);
    }

    public void ShowDialog()
    {
        Time.timeScale = 0;

        npcSideQuestDialogPanel.SetActive(true);
        dialogText.text = GetRandomDialog();
    }

    private string GetRandomDialog()
    {
        if (dialogMessage.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, dialogMessage.Length);
            return dialogMessage[randomIndex];
        }
        return "Bantu aku carikan barangku yang hilang";
    }

    private void CloseDialog()
    {
        Time.timeScale = 1;
        AudioManager.instance.Play("close");
         
        npcSideQuestDialogPanel.SetActive(false);

        NPCSideQuest nPCSideQuest = FindFirstObjectByType<NPCSideQuest>();
        nPCSideQuest. StartQuest();
    }

    public void UpdateQuestUI(string questName, int itemsFound, int itemsToFind, float timeRemaining)
    {
        questPanel.SetActive(true);
        questTitleText.text = questName;
        questProgressText.text = $"Progres: {itemsFound}/{itemsToFind}";
        questTimerText.text = $"Waktu: {Mathf.Ceil(timeRemaining)} detik";
    }

    public void UpdateQuestTimer(float timeRemaining)
    {
        if (questPanel.activeSelf)
        {
            questTimerText.text = $"Waktu: {Mathf.Ceil(timeRemaining)} detik";
        }
    }

    public void ShowQuestFailed()
    {
        questTimerText.text = "";
    }

    public void ClearQuestUI()
    {
        questPanel.SetActive(false);
    }
}
