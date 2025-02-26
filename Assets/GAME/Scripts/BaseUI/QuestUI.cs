using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI questText; 
    public GameObject questPanel;   

    private void Start()
    {
        // Subscribe ke event saat quest harian di-generate
        TakjilQuestManager.Instance.OnDailyQuestGeneratedEvent += UpdateQuestUI;
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        // Periksa apakah quest sudah selesai
        if (TakjilQuestManager.Instance.IsQuestCompleted())
        {
            questText.text = "Quest selesai!";
            return; // Keluar dari metode agar tidak menampilkan quest baru
        }

        if (questText != null)
        {
            questText.text = GenerateQuestText(); 
        }
    }

    public void ShowQuestPanel()
    {
       AudioManager.instance.Play("clicksfx");    
       Time.timeScale = 0;
        
        if (questPanel != null)
        {
            questPanel.SetActive(true); 
        }
    }

    public void HideQuestPanel()
    {
        Time.timeScale = 1;
       
        AudioManager.instance.Play("close");
        if (questPanel != null)
        {
            questPanel.SetActive(false); 
        }
    }

    private string GenerateQuestText()
    {
        string questList = "Takjil yang harus dikumpulkan:\n";

        foreach (var takjil in TakjilQuestManager.Instance.todayTakjilList)
        {
            questList += $"- {takjil.takjilName}\n";
        }

        return questList;
    }
}
