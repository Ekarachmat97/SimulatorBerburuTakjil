using System.Collections.Generic;
using UnityEngine;

public class TakjilQuestManager : MonoBehaviour
{
    public static TakjilQuestManager Instance;

    public List<TakjilData> allTakjilList;
    public List<TakjilData> todayTakjilList = new List<TakjilData>();

    public int questCompletedCount = 0;

    // Event untuk memberi tahu sistem lain kalau quest baru sudah dibuat
    public delegate void OnDailyQuestGenerated();
    public event OnDailyQuestGenerated OnDailyQuestGeneratedEvent;

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
        GenerateDailyTakjilList();
    }

    public void GenerateDailyTakjilList()
    {
        todayTakjilList.Clear();

        // Reset status koleksi takjil
        foreach (var takjil in allTakjilList)
        {
            takjil.isCollected = false;
        }

        // Tentukan jumlah takjil berdasarkan level
        int playerLevel = PlayerManager.Instance.playerLevel;
        int takjilCount;

        // Difficulty scaling by level
        if (playerLevel < 5)
        {
            takjilCount = Random.Range(2, 3);
        }
        else if (playerLevel < 10)
        {
            takjilCount = Random.Range(3, 5);
        }
        else
        {
            takjilCount = Random.Range(4, 6);
        }

        List<TakjilData> availableTakjil = new List<TakjilData>();

        foreach (var takjil in allTakjilList)
        {
            if (takjil.bazarType == BazarType.Default && LapakUnlockManager.Instance.isDefaultUnlocked)
            {
                availableTakjil.Add(takjil);
            }
            else if (takjil.bazarType == BazarType.Modern && LapakUnlockManager.Instance.isModernUnlocked)
            {
                availableTakjil.Add(takjil);
            }
            else if (takjil.bazarType == BazarType.PasarMalam && LapakUnlockManager.Instance.isPasarMalamUnlocked)
            {
                availableTakjil.Add(takjil);
            }
        }

        // Random & variatif
        for (int i = 0; i < takjilCount; i++)
        {
            if (availableTakjil.Count == 0) break;

            int randomIndex = Random.Range(0, availableTakjil.Count);
            todayTakjilList.Add(availableTakjil[randomIndex]);
            availableTakjil.RemoveAt(randomIndex);
        }
        OnDailyQuestGeneratedEvent?.Invoke();
    }

    public bool CheckTakjilCompletion()
    {
        foreach (var takjil in todayTakjilList)
        {
            if (!takjil.isCollected) return false;
        }
        return true;
    }

    public bool IsQuestCompleted()
    {
        return CheckTakjilCompletion();
    }

    public bool IsQuestInProgress()
    {
        // Quest sedang berjalan jika todayTakjilList tidak kosong dan belum selesai
        return todayTakjilList.Count > 0 && !CheckTakjilCompletion();
    }

    public void ResetDailyQuest()
    {
        GenerateDailyTakjilList();
    }
}
