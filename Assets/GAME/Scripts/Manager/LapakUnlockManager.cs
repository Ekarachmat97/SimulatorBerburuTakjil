using UnityEngine;

public class LapakUnlockManager : MonoBehaviour
{
    public static LapakUnlockManager Instance;

    public GameObject pasarModern;
    public GameObject pasarMalam;

    public bool isDefaultUnlocked = true; 
    public bool isModernUnlocked = false;
    public bool isPasarMalamUnlocked = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateLapak();
    }

    public void UpdateLapak()
    {
        int playerLevel = PlayerManager.Instance.playerLevel;

        if (playerLevel >= 5)
        {
            if (!isModernUnlocked)
            {
                isModernUnlocked = true;
                NotificationManager.Instance.ShowNotification("Pasar Modern Terbuka!");
            }
            pasarModern.SetActive(false);
        }
        else
        {
            pasarModern.SetActive(true);
            isModernUnlocked = false;
        }

        if (playerLevel >= 10)
        {
            if (!isPasarMalamUnlocked)
            {
                isPasarMalamUnlocked = true;
                NotificationManager.Instance.ShowNotification("Pasar Malam Terbuka!");
            }
            pasarMalam.SetActive(false);
        }
        else
        {
            pasarMalam.SetActive(true);
            isPasarMalamUnlocked = false;
        }
    }
}
