using UnityEngine;
using System.Collections;

public class NPCSideQuest : MonoBehaviour, IInteractable
{
    [Header("Side Quest setting")]
    public string questName = "Temukan Barang Hilang";
    public int currentQuestLevel = 1;
    public int maxQuestLevel = 5;
    public int itemsToFind = 1;
    [Header("Reward Side Quest")]
    public int rewardCoins = 500;
    public int pahalaReward = 200;
    [Header("Side Quest Duarsi")]
    public float questTimeLimit = 60f;

    private int itemsFound = 0;
    private bool isQuestActive = false;
    private Coroutine questTimerCoroutine;

    private QuestItemSpawner itemSpawner;

    private void Start()
    {
        itemSpawner = FindObjectOfType<QuestItemSpawner>();
    }

    void Update()
    {
        if (isQuestActive && TimeManager.Instance.currentHour >= TimeManager.Instance.questFailHour)
        {
            QuestFailed();
        }
    }

    public void Interact()
    {
        if (!isQuestActive)
        {
            SideQuestUI.Instance. ShowDialog();
           
        }
        else
        {
            CheckQuestProgress();
        }
    }

    public void StartQuest()
    {
        
        if (currentQuestLevel > maxQuestLevel)
        {
            return;
        }

        isQuestActive = true;
        itemsFound = 0;
        itemsToFind = currentQuestLevel; 

        SideQuestUI.Instance.UpdateQuestUI(questName, itemsFound, itemsToFind, questTimeLimit);

        if (itemSpawner != null)
        {
            itemSpawner.SpawnItems(itemsToFind);
        }


        if (questTimerCoroutine != null)
        {
            StopCoroutine(questTimerCoroutine);
        }
        questTimerCoroutine = StartCoroutine(QuestTimer(questTimeLimit));
    }

    private void CheckQuestProgress()
    {
        if (itemsFound >= itemsToFind)
        {
            CompleteQuest();
        }
    
    }

    public void ItemFound()
    {
        if (!isQuestActive) return;

        itemsFound++;
        SideQuestUI.Instance.UpdateQuestUI(questName, itemsFound, itemsToFind, questTimeLimit);

        if (itemsFound >= itemsToFind)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        isQuestActive = false;
        StopCoroutine(questTimerCoroutine);

        if (currentQuestLevel < maxQuestLevel)
        {
            currentQuestLevel++;
            rewardCoins += 50 * currentQuestLevel; 
        }

            pahalaReward = 10 * currentQuestLevel; 

        PlayerManager.Instance.AddCoins(rewardCoins);
        PlayerManager.Instance.AddPahala(pahalaReward);

        // Notifikasi hadiah
        NotificationManager.Instance.ShowNotification($"Kamu mendapatkan {rewardCoins} koin & {pahalaReward} pahala!");

        // Bersihkan UI quest
        SideQuestUI.Instance.ClearQuestUI();
    }


    private IEnumerator QuestTimer(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            SideQuestUI.Instance.UpdateQuestTimer(time);
            yield return null;
        }

        QuestFailed();
    }

    public void QuestFailed()
    {
        isQuestActive = false;

        NotificationManager.Instance.ShowNotification("side Quest Gagal!");
        SideQuestUI.Instance.ShowQuestFailed();
        SideQuestUI.Instance.ClearQuestUI();
    }

    public bool IsQuestActive()
    {
        return isQuestActive;
    }
}
